shader_type canvas_item;

void vertex() {
	VERTEX += vec2(-10.*abs(sin(TIME)), -15.*abs(sin(TIME)));
}
void fragment() {
}
