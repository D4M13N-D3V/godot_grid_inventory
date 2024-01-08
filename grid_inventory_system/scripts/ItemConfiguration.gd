extends Resource
class_name ItemConfiguration
@export var item_id:String = "item"
@export var item_name:String = "Item Name"
@export var item_description:String = "This is the default item description."
@export var item_size:Vector2 = Vector2(3,3)

@export var item_usable:bool = false
@export var item_openable:bool = false
@export var item_droppable:bool = false
@export var item_destroyable:bool = false

@export var item_equipment:bool = false
@export var item_equipment_slot:String = "NONE"
@export var item_texture:Texture2D
