shader_type canvas_item;
render_mode skip_vertex_transform;

uniform mat3 quat;

void vertex() {
	vec3 vertex3D = vec3(VERTEX, 0.0);
	//vertex3D.x += 50.*sin(TIME);
	mat3 rotate = mat3(vec3(0.9, -0.2, 0.0), vec3(0.1, 0.9, 0.), vec3(0.0, 0.0, 1.0));
	vertex3D = vertex3D * quat;
	VERTEX = (MODEL_MATRIX * vec4(vertex3D, 1.0)).xy;
}

void fragment() {
	
}
