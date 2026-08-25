shader_type canvas_item;

uniform sampler2D mask_texture : hint_default_white;
uniform sampler2D gradient_texture;
uniform vec4 my_color : source_color = vec4(1.0);
uniform bool invert_mask = true;
uniform vec2 shadow_offset = vec2(4.0, 4.0);
uniform vec4 shadow_color : source_color = vec4(0.0, 0.0, 0.0, 0.4);

void fragment() {
    vec4 tex = texture(TEXTURE, UV);
    float mask = texture(mask_texture, UV).r;
	
    if (invert_mask) mask = 1.0 - mask;
	
    vec4 gradient = texture(gradient_texture, UV);
    COLOR.rgb = tex.rgb * gradient.rgb * my_color.rgb;
    COLOR.a = tex.a * mask * gradient.a * my_color.a;
}