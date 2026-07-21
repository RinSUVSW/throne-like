extends Node2D


@export var _anim: AnimationPlayer
@export var _area: Area2D
@export var _speed: float = 1.0
@export var _delay_timer: Timer
var player: Node2D


func _ready() -> void:
	_area.body_entered.connect(_body_entered)

	_set_collision_enabled(false)
	visible = false
	_speed = 0.0


func _process(delta: float) -> void:
	position += Vector2(cos(rotation), sin(rotation)) * _speed * delta * 60.0


func _body_entered(_other) -> void:
	_set_collision_enabled(false)
	_speed = 0.0
	_anim.play("die")


func setup(p: Node2D, info: BulletSpawnPoint.BulletInfo) -> void:
	player = p

	_delay_timer.wait_time = 0.01 + info.index_of_this_type * 0.05
	_delay_timer.timeout.connect(func():
		_set_collision_enabled(true)
		visible = true
		_speed = 1.0
		rotation = player.rotation
		global_position = player.global_position

		if info.global_index % 3 == 2:
			modulate = Color.RED
	)
	_delay_timer.start()


func _set_collision_enabled(b: bool) -> void:
	_area.set_deferred("monitorable", b)
	_area.set_deferred("monitoring", b)
