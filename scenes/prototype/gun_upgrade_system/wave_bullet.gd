extends Bullet


@export var _die_particle: PackedScene
@export var _area: Area2D
var _angle: float = 0.0
var _speed: float = 3.5
@onready var _elapsed: float = randf() * PI * 2.0


func _ready() -> void:
	_area.body_entered.connect(func(__):
		var p = _die_particle.instantiate()
		get_tree().current_scene.add_child(p)
		p.global_position = $Offset.global_position
		p.modulate = modulate
		queue_free()
	)

	fired.connect(func():
		position = player.global_position
		_angle = player.rotation
	)


func _process(delta: float) -> void:
	position += Vector2(cos(_angle), sin(_angle)) * _speed * delta * 60.0

	_elapsed += delta * 20.0
	var s = sin(_elapsed) * 15.0

	$Offset.position = Vector2(
		s * cos(_angle + PI * 0.5),
		s * sin(_angle + PI * 0.5),
	)


func get_fire_delay() -> float:
	return 0.0
