shader_type canvas_item;

uniform sampler2D newImage;

void fragment() {
	vec4 textureColor = texture(newImage, UV);
	COLOR = textureColor;
}
