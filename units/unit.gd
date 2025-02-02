class_name Unit
extends CharacterBody2D

var commands: Array[Command] = []

var health_component: HealthComponent

var selected: bool = false
@export var icon: Texture2D
