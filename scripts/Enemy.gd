extends CharacterBody2D

signal enemy_killed

@export var move_speed: float = 130.0
@export var max_health: int = 45
@export var contact_damage: int = 8
@export var attack_cooldown: float = 0.7

var health: int
var _hero: CharacterBody2D
var _hit_cd: float = 0.0

func _ready() -> void:
	health = max_health
	add_to_group("damageable")
	_hero = get_tree().get_first_node_in_group("hero") as CharacterBody2D
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

	if global_position.distance_to(_hero.global_position) <= 28.0 and _hit_cd <= 0.0:
		if _hero.has_method("take_damage"):
			_hero.take_damage(contact_damage)
		_hit_cd = attack_cooldown

func take_damage(amount: int) -> void:
	health -= amount
	if health <= 0:
		enemy_killed.emit()
		queue_free()

func _draw() -> void:
	draw_circle(Vector2.ZERO, 14.0, Color(0.9, 0.2, 0.2))
