extends Control
class_name InventoryController
const ITEM_BASE = preload("res://grid_inventory_system/scenes/Item.tscn")
const CONTEXT_MENU = preload("res://grid_inventory_system/scenes/ContextMenu.tscn")
@export var inventory_open_input:String = "inventory_open"
@export var inventory_close_input:String = "inventory_close"
@export var inventory_use_input:String = "inventory_use"
@export var inventory_grab_input:String = "inventory_grab"
@export var inventory_rotate_input:String = "inventory_rotate"
@export var inventory_grid:Control
@export var inventory_loot_grid:Control
@export var inventory_background:Control
@export var inventory_equipment_slots:Control


var inventory_open = true
var inventory_item_dragged = null
var inventory_item_cursor_offset = Vector2()
var inventory_item_dragged_last_container = null
var inventory_item_dragged_last_pos = Vector2()
var inventory_context_menu:Control = null
var inventory_context_menu_item


signal inventory_item_equipped(item_config,slot)
signal inventory_item_unequipped(item_config,slot)
signal inventory_item_dropped(item_config)
signal inventory_item_grabbed(item_config, x, y, container)
signal inventory_item_released(item_config, x, y, container)
signal inventory_item_used(item_config, x, y)
signal inventory_item_destroyed(item_config, x, y)
signal inventory_item_opened(item_config, x, y)

func _ready():
	if(inventory_grid==null):
		printerr("The inventory grid was not assigned to the inventory controller!")
	if(inventory_background==null):
		printerr("The inventory background was not assigned to the inventory controller!")
	if(inventory_grid==null):
		printerr("The inventory equipment slots was not assigned to the inventory controller!")

func _process(delta):
	if(Input.is_action_just_pressed(inventory_open_input) and inventory_open==false):
		inventory_open = true
		inventory_background.visible=true
	
	if(Input.is_action_just_pressed(inventory_close_input) and inventory_open==true):
		inventory_open = false
		inventory_background.visible=false
		
	if(Input.is_action_just_pressed(inventory_rotate_input) and inventory_open==true and inventory_item_dragged != null):
		inventory_item_dragged.rotate_item(90)
		
	var cursor_pos = get_global_mouse_position()
	
	if Input.is_action_just_pressed("inventory_grab"):	
		grab(cursor_pos)
			
	if Input.is_action_just_released("inventory_grab"):
		release(cursor_pos)
		
	if Input.is_action_just_pressed("inventory_use"):
		try_to_open_context_menu()
	
	
	if inventory_item_dragged != null:
		inventory_item_dragged.global_position = cursor_pos + inventory_item_cursor_offset

func try_to_open_context_menu():
	var mouse_position = get_global_mouse_position()
	var c = _get_container_mouse_over()
	if c != null and c.has_method("grab_item"):
		var item = c.check_item(mouse_position)
		if item != null:
			if(inventory_context_menu!=null):
				inventory_context_menu.queue_free()
			show_context_menu(mouse_position,item)
		else:
			hide_context_menu()
	

# Function to show the context menu at a specific position
func show_context_menu(mouse_pos: Vector2, item):
	inventory_context_menu = CONTEXT_MENU.instantiate()
	inventory_background.add_child(inventory_context_menu)
	inventory_context_menu.global_position = mouse_pos + Vector2(-20,-15)
	inventory_context_menu.set_use(not item.item_config.item_usable)
	inventory_context_menu.set_drop(not item.item_config.item_droppable)
	inventory_context_menu.set_destroy(not item.item_config.item_destroyable)
	inventory_context_menu.set_open(not item.item_config.item_openable)
	inventory_context_menu_item = item
# Function to hide the context menu
func hide_context_menu():
	if(inventory_context_menu!=null):
		inventory_context_menu_item=null
		inventory_context_menu.queue_free()

func context_menu_open():
	print("OPEN!")
	
func context_menu_use():
	print("USED!")

func context_menu_destroy():
	print("DESTROYED!")
	
func context_menu_drop():
	print("DROPPED!")

func grab(cursor_pos):
	var c = _get_container_mouse_over()
	if c != null and c.has_method("grab_item"):
		inventory_item_dragged = c.grab_item(cursor_pos)
		if inventory_item_dragged != null:
			add_child(inventory_item_dragged)
			inventory_item_dragged_last_container = c
			inventory_item_dragged_last_pos = inventory_item_dragged.global_position
			inventory_item_cursor_offset = inventory_item_dragged.global_position - cursor_pos
			if(inventory_context_menu_item==inventory_item_dragged):
				release(cursor_pos)

func release(cursor_pos):
	if inventory_item_dragged == null:
		return
	var c = _get_container_mouse_over()
	if c == null:
		drop_item()
	elif c.has_method("insert_item"):
		if c.insert_item(inventory_item_dragged):
			inventory_item_dragged = null
		else:
			return_item()
	else:
		return_item()

func use(cursor_pos):
	pass

func pickup_item(item_id):
	var item = ITEM_BASE.instantiate()
	item.set_meta("id", item_id)
	item.init_item(item_id)
	if not inventory_grid.insert_item_at_first_available_spot(item):
		item.queue_free()
		return false
	return true
	
func drop_item():
	inventory_item_dropped.emit(inventory_item_dragged.item_config)
	inventory_item_dragged.queue_free()
	inventory_item_dragged = null

# Function to return the dragged item to its original position or container
func return_item():
	inventory_item_dragged.global_position = inventory_item_dragged_last_pos
	inventory_item_dragged_last_container.insert_item(inventory_item_dragged)
	inventory_item_dragged = null
	
func _get_container_mouse_over():
	
	if(_is_mouse_ontop_of_control(inventory_grid)==true):
		return inventory_grid
	elif(_is_mouse_ontop_of_control(inventory_loot_grid)==true):
		return inventory_loot_grid
	elif(_is_mouse_ontop_of_control(inventory_equipment_slots)==true):
		return inventory_equipment_slots
	elif(_is_mouse_ontop_of_control(inventory_background)==true):
		return inventory_background
	return null

func _is_mouse_ontop_of_control(c):
	var cursor_pos = get_global_mouse_position()
	return c.get_global_rect().has_point(cursor_pos)
