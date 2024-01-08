extends Control

enum slot_state { free, used }

@export var in_use = false

func used():
	in_use  = true

func free():
	in_use = false
