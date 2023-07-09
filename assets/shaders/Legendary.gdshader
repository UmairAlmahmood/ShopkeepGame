shader_type canvas_item;
uniform sampler2D noiseTexture;
uniform sampler2D noiseTexture2;
void fragment() {
	vec4 textureColor = texture(TEXTURE, UV);
	vec2 distortedUV = UV;
	distortedUV.x = distortedUV.y*0.2;
	vec2 scroll = vec2(0.1, 0.1);
	float noiseColor = texture(noiseTexture, UV + (scroll * sin(TIME/1.2))).r;
	float noiseColor2 = texture(noiseTexture2, UV + scroll*2.*cos(TIME/1.4)).r;
	float normalizedNoise = noiseColor * noiseColor2;
	float isMetal = step(0.3, textureColor.r) * step(textureColor.r, 0.6) 
	* step(textureColor.g, 0.6) * step(0.3, textureColor.g) 
	* step(textureColor.b, 0.7) * step(0.4, textureColor.b);

	COLOR = mix(textureColor, vec4(vec3(normalizedNoise), textureColor.a), (.6*sin(TIME)-0.45));

}
