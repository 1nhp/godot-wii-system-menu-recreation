shader_type canvas_item;

uniform float offset : hint_range(0.0, 10.0) = 2.0;
uniform float opacity : hint_range(0.0, 1.0) = 1.0;

void fragment() {
    vec2 o = TEXTURE_PIXEL_SIZE * offset;

    vec4 color = vec4(0.0);

    color += texture(TEXTURE, UV + vec2(-o.x, -o.y));
    color += texture(TEXTURE, UV + vec2( o.x, -o.y));
    color += texture(TEXTURE, UV + vec2(-o.x,  o.y));
    color += texture(TEXTURE, UV + vec2( o.x,  o.y));

    COLOR = color * 0.25;
    COLOR.a *= opacity;
}