extends CharacterBody3D

signal hit_obstacle

@export var speed: float = 10.0
@export var jump_velocity: float = 7.0
@export var gravity: float = 22.0
@export var mouse_sensitivity: float = 0.0025

@onready var pivot: Node3D = $Pivot
@onready var camera: Camera3D = $Pivot/Camera3D

var score: int = 0
var alive: bool = true

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and alive:
		rotate_y(-event.relative.x * mouse_sensitivity)
		pivot.rotate_x(-event.relative.y * mouse_sensitivity)
		pivot.rotation.x = clamp(pivot.rotation.x, -1.2, 1.2)
	if event.is_action_pressed("ui_cancel"):
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)

func _physics_process(delta: float) -> void:
	if not alive:
		velocity = Vector3.ZERO
		move_and_slide()
		return

	if not is_on_floor():
		velocity.y -= gravity * delta
	else:
		if Input.is_action_just_pressed("jump"):
			velocity.y = jump_velocity

	var input_dir := Input.get_vector("move_left", "move_right", "move_forward", "move_back")
	var move_dir := (transform.basis * Vector3(input_dir.x, 0.0, input_dir.y)).normalized()

	if move_dir:
		velocity.x = move_dir.x * speed
		velocity.z = move_dir.z * speed
	else:
		velocity.x = move_toward(velocity.x, 0.0, speed)
		velocity.z = move_toward(velocity.z, 0.0, speed)

	move_and_slide()

func collect_pickup() -> void:
	score += 1

func die() -> void:
	if not alive:
		return
	alive = false
	hit_obstacle.emit()
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
