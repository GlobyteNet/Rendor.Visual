### VERTEX SHADER

#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aPoint;
layout (location = 2) in vec4 aColor;

out vec4 ourColor;

uniform vec2 u_Resolution;

void main()
{
	gl_Position = vec4(aPos.x / u_Resolution.x * 2.0 - 1.0, 1.0 - aPos.y / u_Resolution.y * 2.0, aPos.z, 1.0);
	ourColor = aColor;
}

### END VERTEX SHADER
### FRAGMENT SHADER

#version 330 core

in vec4 ourColor;

out vec4 FragColor;
// uniform vec4 u_Color;

void main()
{
	FragColor = ourColor;
}

### END FRAGMENT SHADER