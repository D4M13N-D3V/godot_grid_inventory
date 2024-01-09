extends Node


@export var inventory:InventoryController
@export var grid:GridContainer
func _ready():
	InventoryManager.connect("inventory_item_picked_up", item_picked_up)
	var items = InventoryManager.cached_items
	for item in items:
		var button = Button.new()
		grid.add_child(button)
		button.text = "Spawn "+item
		button.connect("pressed", func():
			inventory.pickup_item(item))
			
func item_picked_up(grid_name, x, y, screen_x, screen_y, item_control):
	print(item_control.item_config.item_name)

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
