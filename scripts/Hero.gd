extends CharacterBody2D

signal hero_died

@export var move_speed: float = 250.0
@export var max_health: int = 100
@export var attack_damage: int = 25
@export var attack_range: float = 70.0
@export var attack_cooldown: float = 0.35

var health: int
var _cooldown_left: float = 0.0

@onready var hp_label: Label = get_node_or_null("../UI/HeroHp")

func _ready() -> void:
	health = max_health
	add_to_group("hero")
	queue_redraw()
	_update_hp_label()

func _physics_process(delta: float) -> void:
	var move_dir := Input.get_vector("move_left", "move_right", "move_up", "move_down")
	velocity = move_dir * move_speed
	move_and_slide()

	if _cooldown_left > 0.0:
		_cooldown_left -= delta

	if Input.is_action_just_pressed("attack") and _cooldown_left <= 0.0:
		attack()
		_cooldown_left = attack_cooldown

func attack() -> void:
	for target in get_tree().get_nodes_in_group("damageable"):
		if target == self:
			continue
		if not target.has_method("take_damage"):
			continue
		if global_position.distance_to(target.global_position) <= attack_range:
			target.take_damage(attack_damage)

func take_damage(amount: int) -> void:
	health -= amount
	_update_hp_label()
	if health <= 0:
		health = 0
		hero_died.emit()
		queue_free()

func heal(amount: int) -> void:
	health = min(max_health, health + amount)
	_update_hp_label()

func _update_hp_label() -> void:
	if hp_label:
		hp_label.text = "HP героя: %d / %d" % [health, max_health]

func _draw() -> void:
	draw_circle(Vector2.ZERO, 18.0, Color(0.1, 0.55, 1.0))
