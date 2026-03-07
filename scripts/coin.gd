extends Area3D

@onready var mesh: MeshInstance3D = $MeshInstance3D

func _process(delta: float) -> void:
	rotate_y(2.8 * delta)
	mesh.position.y = 0.45 + sin(Time.get_ticks_msec() * 0.004) * 0.15
