shader_type canvas_item;

// Define the frequency and phase values to control the rainbow effect
uniform float frequency = 1.0; // Adjust the frequency for the speed of the rainbow
const float phaseR = 0.0;
const float phaseG = 2.0;
const float phaseB = 4.0;

void fragment() {
	vec4 texColor = texture(TEXTURE, UV + vec2(-1.0, 0.0) * TIME);

    // Calculate color values using sine and cosine functions
    float red = 0.5 + 0.5 * sin(frequency * TIME + phaseR);
    float green = 0.5 + 0.5 * sin(frequency * TIME + phaseG);
    float blue = 0.5 + 0.5 * sin(frequency * TIME + phaseB);

    // Combine color values to create the rainbow effect
    vec3 rainbowColor = vec3(red, green, blue);

    // Output the rainbow color
    COLOR = texColor * vec4(rainbowColor, 0.75);
}