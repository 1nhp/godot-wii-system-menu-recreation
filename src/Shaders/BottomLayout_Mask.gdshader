shader_type canvas_item;

uniform sampler2D mask_texture;
uniform bool invert = true;

void fragment() {
    // Normalizes mask sampling to match the current TEXTURE bounds
    vec2 mask_uv = UV; 
    
    // Sample the mask texture using the corrected UV
    vec4 mask_color = texture(mask_texture, mask_uv);
         
    float alpha = mask_color.a;
    if (invert) {
        alpha = 1.0 - alpha;
    }
         
    vec4 tex_color = texture(TEXTURE, UV);
    COLOR = vec4(tex_color.rgb, tex_color.a * alpha);
}
