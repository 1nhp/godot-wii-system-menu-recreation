shader_type canvas_item;

uniform sampler2D gradient_texture;
uniform sampler2D gradient_mask;
uniform sampler2D main_mask;

uniform vec4 my_color : source_color = vec4(1.0);

uniform vec4 target_color : source_color;
uniform vec4 replacement_color : source_color;
uniform float tolerance : hint_range(0.0, 1.0) = 0.1;

void fragment()
{
    vec4 tex = texture(TEXTURE, UV);

    float gradient_area = texture(gradient_mask, UV).r;
    float main_area = texture(main_mask, UV).r;
    vec4 gradient = texture(gradient_texture, UV);
    vec3 base_color = tex.rgb;

    if (distance(base_color, target_color.rgb) <= tolerance)
    {
        base_color = replacement_color.rgb;
    }
    vec3 gradient_color = base_color * gradient.rgb;

    vec3 final_color = mix(
        base_color,
        gradient_color,
        gradient_area
    );

    COLOR.rgb = final_color * my_color.rgb;
    COLOR.a = tex.a * main_area * my_color.a;
}