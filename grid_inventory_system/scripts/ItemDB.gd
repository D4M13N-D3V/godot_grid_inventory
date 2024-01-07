extends Node

# Dictionary to store loaded items with their item_id as the key
var ITEMS = {}

func _ready():
	# Load items from the "items" directory and populate the ITEMS dictionary
	for i in DirAccess.get_files_at("res://grid_inventory_system/items"):
		if(i.ends_with(".tres")):
			var item = load("res://grid_inventory_system/items/" + i)
		ITEMS[item.item_id] = item

# Function to retrieve an item based on its item_id
func get_item(item_id):
	# Check if the item_id exists in the ITEMS dictionary
	if ITEMS.has(item_id):
		return ITEMS[item_id]
	else:
		return null
