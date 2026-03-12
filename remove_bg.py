import sys
import subprocess
try:
    import png
except ImportError:
    subprocess.check_call([sys.executable, "-m", "pip", "install", "pypng"])
    import png

path = "frontend/src/assets/logo.png"
r = png.Reader(path)
w, h, pixels, metadata = r.read_flat()
pixels = list(pixels)

has_alpha = metadata.get('alpha', False)
planes = metadata.get('planes', 3)

bg_r, bg_g, bg_b = pixels[0], pixels[1], pixels[2]

new_pixels = []
for i in range(0, len(pixels), planes):
    r_val, g_val, b_val = pixels[i], pixels[i+1], pixels[i+2]
    a_val = pixels[i+3] if has_alpha else 255
    
    # Tolerance for background removal
    if abs(r_val - bg_r) < 15 and abs(g_val - bg_g) < 15 and abs(b_val - bg_b) < 15:
        a_val = 0
        
    new_pixels.append(r_val)
    new_pixels.append(g_val)
    new_pixels.append(b_val)
    new_pixels.append(a_val)

new_metadata = dict(metadata)
new_metadata['alpha'] = True

with open("frontend/src/assets/logo_transparent.png", "wb") as f:
    writer = png.Writer(w, h, **{k:new_metadata[k] for k in new_metadata if k not in ['planes', 'background', 'transparent', 'bitdepth'] and k in ['width', 'height', 'greyscale', 'alpha']})
    # Since original might have had different bitdepths, let's keep it simple.
    writer.write_array(f, new_pixels)

import os
os.replace("frontend/src/assets/logo_transparent.png", "frontend/src/assets/logo.png")
print("Successfully generated transparent logo!")
