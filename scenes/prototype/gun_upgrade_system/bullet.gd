class_name Bullet
extends Node2D
## something the player can fire. receives the player and a `BulletInfo` instance,
## then the `fired` signal emits.
## can optionally add a delay before the shot actually goes off by changing the
## value `get_fire_delay()` returns.
##
## NOTE: doesn't come with collision by default.


signal fired


var player: Node2D
var info: BulletSpawnPoint.BulletInfo


## should be called directly after this gets added to the scene tree, when the
## player pulls the trigger (but potentially before it actually activates).
func setup(player_: Node2D, info_: BulletSpawnPoint.BulletInfo) -> void:
	player = player_
	info = info_
	global_position = player.global_position

	var delay: float = get_fire_delay()

	if delay != 0:
		await get_tree().create_timer(delay).timeout

	await get_tree().create_timer(delay).timeout
	fired.emit()


## override this and a return a float to optionally add some delay to the shot,
## in seconds.
func get_fire_delay() -> float:
	return info.index_of_this_type * 0.25