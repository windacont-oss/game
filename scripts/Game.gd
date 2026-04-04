extends Node2D

@onready var hero: CharacterBody2D = $Hero
@onready var boss: CharacterBody2D = $Boss
@onready var enemies: Array[Node] = [$Enemy1, $Enemy2, $Enemy3]
@onready var status_label: Label = $UI/Status

func _ready() -> void:
	status_label.text = "Управление: WASD, удар: Space"
	hero.hero_died.connect(_on_hero_died)
	boss.boss_killed.connect(_on_boss_killed)
	for enemy in enemies:
		enemy.enemy_killed.connect(_on_enemy_killed)

func _on_enemy_killed() -> void:
	var alive := 0
	for enemy in enemies:
		if is_instance_valid(enemy):
			alive += 1
	if alive == 0 and is_instance_valid(boss):
		status_label.text = "Все враги повержены. Добей босса!"

func _on_boss_killed() -> void:
	status_label.text = "ПОБЕДА! Босс уничтожен."
	get_tree().paused = true

func _on_hero_died() -> void:
	status_label.text = "ПОРАЖЕНИЕ! Герой пал."
	get_tree().paused = true
