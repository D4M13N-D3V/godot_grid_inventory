extends Control
class_name ItemGraphic
@export var item_config:ItemConfiguration
@export var graphic:TextureRect

func init_item(item_id):
	var dbItem = InventoryManager.get_item(item_id)
	item_config = dbItem
	graphic.set_size(Vector2(dbItem.item_size.x, dbItem.item_size.y))
	set_size(Vector2(dbItem.item_size.x, dbItem.item_size.y))
	graphic.texture = dbItem.item_texture
	graphic.pivot_offset = Vector2(graphic.size.x/2.0, graphic.size.y/2.0)
	pivot_offset = Vector2(graphic.size.x/2.0, graphic.size.y/2.0)
	graphic.position = Vector2()
	print("TEST")

func rotate_item(amount):
	graphic.rotation_degrees = graphic.rotation_degrees+amount
	if(graphic.rotation_degrees==90 or graphic.rotation_degrees==270):
		set_size(Vector2(item_config.item_size.y, item_config.item_size.x))
	else:
		set_size(Vector2(item_config.item_size.x, item_config.item_size.y))
	
	if(graphic.rotation_degrees==360):
		graphic.rotation_degrees=0
