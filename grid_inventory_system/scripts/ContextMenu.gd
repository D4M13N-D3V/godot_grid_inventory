extends Panel
class_name ContextMenu

@export var use_button:Button
@export var open_button:Button
@export var drop_button:Button
@export var destroy_button:Button

func set_use(disabled):
	use_button.disabled=disabled

func set_open(disabled):
	open_button.disabled=disabled

func set_drop(disabled):
	drop_button.disabled=disabled

func set_destroy(disabled):
	destroy_button.disabled=disabled
