extends Bullet


@export var _basic_bullet_scene: PackedScene
var _spread_degrees: float = 45.0


func _ready() -> void:
	fired.connect(func():
		_fire_bullet()
		_fire_bullet()
		_fire_bullet()
		_fire_bullet()
	)


func _fire_bullet() -> void:
	var b: Bullet = _basic_bullet_scene.instantiate()
	player.fire_bullet(b)

	b.fired.connect(func():
		b.rotation_degrees += (randf() * _spread_degrees * 2.0) - _spread_degrees
	)
