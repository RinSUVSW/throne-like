class_name BulletSpawnPoint
extends Node2D


@export var bullet_types: Array[BulletResource]
var _global_index: int = 0
var _angle: float = 0.0
var _turn_speed: float = 0.1
var _speed: float = 1.0


func _process(delta: float) -> void:
	rotation = _angle
	var dt = delta * 60.0
	var vel = Vector2(cos(_angle), sin(_angle)) * _speed * dt

	if Input.is_action_pressed("move_up"):
		position += vel
	elif Input.is_action_pressed("move_down"):
		position -= vel

	if Input.is_action_pressed("move_left"):
		_angle -= _turn_speed * dt
	elif Input.is_action_pressed("move_right"):
		_angle += _turn_speed * dt


func _input(event: InputEvent) -> void:
	if not event.is_action_pressed("Fire"):
		return

	for i in bullet_types.size():
		var res: BulletResource = bullet_types[i]
		var b: Node2D = res.scene.instantiate()
		get_tree().current_scene.add_child(b)

		var info = BulletInfo.new()
		info.index = i
		info.index_of_this_type = bullet_types.slice(0, i).count(res)
		info.global_index = _global_index

		b.setup(self, info)
		_global_index += 1


class BulletInfo:
	## which bullet is being fired, as a persistent global counter. if you fire
	## 3 separate shots, this is 3.
	var global_index: int
	## the ordered number of the bullet being fired *during this shot.*
	var index: int
	## the ordered number of the bullet being fired during this shot, of this
	## specific type. if 4 basic bullets are shot, then 1 shotgun shot, then
	## another basic bullet, this is 5.
	var index_of_this_type: int
