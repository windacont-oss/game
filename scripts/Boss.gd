extends CharacterBody2D

signal boss_killed

@export var move_speed: float = 95.0
@export var max_health: int = 260
@export var contact_damage: int = 18
@export var attack_cooldown: float = 0.5

var health: int
var _hero: CharacterBody2D
var _hit_cd: float = 0.0

@onready var boss_hp_label: Label = get_node_or_null("../UI/BossHp")

func _ready() -> void:
	health = max_health
	add_to_group("damageable")
	_hero = get_tree().get_first_node_in_group("hero") as CharacterBody2D
	_update_label()
	queue_redraw()

func _physics_process(delta: float) -> void:
	if _hit_cd > 0.0:
		_hit_cd -= delta

	if not is_instance_valid(_hero):
		_hero = get_tree().get_first_node_in_group("hero") as CharacterBody2D
		velocity = Vector2.ZERO
		move_and_slide()
		return

	var dir := (_hero.global_position - global_position).normalized()
	velocity = dir * move_speed
	move_and_slide()

	if global_position.distance_to(_hero.global_position) <= 38.0 and _hit_cd <= 0.0:
		if _hero.has_method("take_damage"):
			_hero.take_damage(contact_damage)
		_hit_cd = attack_cooldown

func take_damage(amount: int) -> void:
	health -= amount
	_update_label()
	if health <= 0:
		boss_killed.emit()
		queue_free()

func _update_label() -> void:
	if boss_hp_label:
		boss_hp_label.text = "HP босса: %d / %d" % [max(health, 0), max_health]

func _draw() -> void:
	draw_circle(Vector2.ZERO, 26.0, Color(0.5, 0.0, 0.7))
