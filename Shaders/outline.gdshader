shader_type canvas_item;

uniform vec4 outline_color = vec4(1.0, 1.0, 1.0, 1.0); // Default color value
uniform float outline_thickness : hint_range(0.0, 10.0) = 1.0; // Customizable outline thickness

void fragment() {
    float size_x = outline_thickness / float(textureSize(TEXTURE, 0).x);
    float size_y = outline_thickness / float(textureSize(TEXTURE, 0).y);
    vec4 sprite_color = texture(TEXTURE, UV);
    float alpha = -8.0 * sprite_color.a;
    alpha += texture(TEXTURE, UV + vec2(size_x, 0)).a;
    alpha += texture(TEXTURE, UV + vec2(-size_x, 0)).a;
    alpha += texture(TEXTURE, UV + vec2(0, size_y)).a;
    alpha += texture(TEXTURE, UV + vec2(0, -size_y)).a;
    
    alpha += texture(TEXTURE, UV + vec2(size_x, size_y)).a;
    alpha += texture(TEXTURE, UV + vec2(-size_x, size_y)).a;
    alpha += texture(TEXTURE, UV + vec2(-size_x, -size_y)).a;
    alpha += texture(TEXTURE, UV + vec2(size_x, -size_y)).a;
    vec4 final_color = mix(sprite_color, outline_color, clamp(alpha, 0.0, 1.0));
    COLOR = vec4(final_color.rgb, clamp(abs(alpha) + sprite_color.a, 0.0, 1.0));
}
