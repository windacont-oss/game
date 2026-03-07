extends CanvasLayer

@onready var objective_label: Label = $MarginContainer/VBoxContainer/Objective
@onready var score_label: Label = $MarginContainer/VBoxContainer/Score
@onready var state_label: Label = $CenterContainer/State

func update_score(score: int, total: int) -> void:
	score_label.text = "Кристаллы: %d / %d" % [score, total]

func update_objective(text: String) -> void:
	objective_label.text = text

func show_message(text: String) -> void:
	state_label.text = text
	state_label.visible = true

func hide_message() -> void:
	state_label.visible = false
