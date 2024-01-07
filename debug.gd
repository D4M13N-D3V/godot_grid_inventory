extends Node


func _on_button_3_pressed():
	$"..".pickup_item("small_debug_item")


func _on_button_2_pressed():
	$"..".pickup_item("medium_debug_item")


func _on_button_pressed():
	$"..".pickup_item("large_debug_item")


func _on_button_4_pressed():
	$"..".pickup_item("shirt")


func _on_button_5_pressed():
	$"..".pickup_item("hat")
