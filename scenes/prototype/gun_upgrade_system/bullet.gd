extends Node2D


signal fired


@export var _area: Area2D
var player: Node2D
var info: BulletSpawnPoint.BulletInfo


func _ready() -> void:
	_area.body_entered.connect(_body_entered)
	_set_collision_enabled(false)


func _body_entered(_other) -> void:
	# TODO: ways to make a bullet pierce? maybe by having an HP and when it gets
	# set to 0 it's freed?
	queue_free()


func setup(player_: Node2D, info_: BulletSpawnPoint.BulletInfo) -> void:
	player = player_
	info = info_

	var delay: float = get_fire_delay()

	if delay != 0:
		await get_tree().create_timer(delay).timeout

	await get_tree().create_timer(0.01 + info.index_of_this_type * 0.05).timeout
	_set_collision_enabled(true)
	fired.emit()


## override this and a return a float to optionally add some delay to the shot,
## in seconds.
func get_fire_delay() -> float:
	return info.index_of_this_type * 0.05


func _set_collision_enabled(b: bool) -> void:
	_area.set_deferred("monitorable", b)
	_area.set_deferred("monitoring", b)
