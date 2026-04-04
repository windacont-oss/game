extends Area2D

@export var heal_amount: int = 35

func _ready() -> void:
	body_entered.connect(_on_body_entered)
	queue_redraw()

func _on_body_entered(body: Node) -> void:
	if body.has_method("heal"):
		body.heal(heal_amount)
		queue_free()

func _draw() -> void:
	draw_circle(Vector2.ZERO, 10.0, Color(0.1, 0.9, 0.3))
