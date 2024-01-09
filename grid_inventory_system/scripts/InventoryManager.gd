extends Node


# Dictionary to store loaded items with their item_id as the key
var cached_items = {}

func _ready():
	# Load items from the "items" directory and populate the ITEMS dictionary
	for i in DirAccess.get_files_at("res://grid_inventory_system/items"):
		if(i.ends_with(".tres")):
			var item = load("res://grid_inventory_system/items/" + i)
			cached_items[item.item_id] = item

# Function to retrieve an item based on its item_id
func get_item(item_id):
	# Check if the item_id exists in the ITEMS dictionary
	if cached_items.has(item_id):
		return cached_items[item_id]
	else:
		return null

#signal inventory_open
#signal inventory_close
signal inventory_item_picked_up(grid_name, x, y, screen_x, screen_y, item_control)
#signal inventory_item_used
#signal inventory_item_opened
#signal inventory_item_dropped
#signal inventory_item_destroyed
#signal inventory_item_dragged(screex_x, screen_y, grid_x, grid_y, )
#signal inventory_item_released(screen_x, screen_y, grid_x, grid_y, slot_control, grid_control, item_control)
