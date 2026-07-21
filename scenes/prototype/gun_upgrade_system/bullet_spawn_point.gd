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
	if event.is_action_pressed("Fire"):
		_fire()


## perform the action "fire gun's." this pulls the trigger and fires everything
## the player has on hand.
func _fire() -> void:
	for i in bullet_types.size():
		var res: BulletResource = bullet_types[i]
		var b: Node2D = res.scene.instantiate()
		fire_bullet(b, i, res)


## fire a single bullet.
## can optionally pass in index and bullet_resource, which will be used to build
## a BulletInfo. if those aren't passed, the BulletInfo will have incomplete
## information.
func fire_bullet(bullet: Node2D, index: int = 0, bullet_resource = null) -> void:
	get_tree().current_scene.add_child(bullet)

	var info = BulletInfo.new()
	info.index = index
	info.index_of_this_type = bullet_types.slice(0, index).count(bullet_resource)
	info.global_index = _global_index

	bullet.setup(self, info)
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
