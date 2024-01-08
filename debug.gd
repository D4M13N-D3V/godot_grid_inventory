extends Node


@export var inventory:InventoryController
@export var grid:GridContainer
func _ready():
	var items = ItemDb.ITEMS
	for item in items:
		var button = Button.new()
		grid.add_child(button)
		button.text = "Spawn "+item
		button.connect("pressed", func():
			inventory.pickup_item(item))


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
