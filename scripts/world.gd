extends Node3D

@onready var player: CharacterBody3D = $Player
@onready var hud: CanvasLayer = $HUD
@onready var pickups: Node3D = $Pickups
@onready var finish_gate: Area3D = $FinishGate

var total_pickups: int = 0
var game_finished: bool = false

func _ready() -> void:
	total_pickups = pickups.get_child_count()
	player.hit_obstacle.connect(_on_player_hit)
	for pickup in pickups.get_children():
		pickup.body_entered.connect(_on_pickup_collected.bind(pickup))
	finish_gate.body_entered.connect(_on_finish_entered)
	hud.update_score(0, total_pickups)
	hud.update_objective("Собери все кристаллы и дойди до портала")
	hud.hide_message()

func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("restart"):
		get_tree().reload_current_scene()

func _on_pickup_collected(body: Node3D, pickup: Area3D) -> void:
	if game_finished or body != player:
		return
	player.collect_pickup()
	pickup.queue_free()
	hud.update_score(player.score, total_pickups)
	if player.score == total_pickups:
		hud.update_objective("Все кристаллы собраны! Беги к порталу!")

func _on_finish_entered(body: Node3D) -> void:
	if game_finished or body != player:
		return
	if player.score < total_pickups:
		hud.show_message("Собери все кристаллы перед выходом")
		return
	game_finished = true
	hud.update_objective("Победа!")
	hud.show_message("Ты победил! Нажми R для перезапуска")
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)

func _on_player_hit() -> void:
	if game_finished:
		return
	game_finished = true
	hud.show_message("Ты врезался в ловушку! Нажми R для перезапуска")
	hud.update_objective("Попробуй ещё раз")
