import sys
try:
    from PIL import Image
    from PIL import ImageColor
except ImportError:
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "Pillow"])
    from PIL import Image
    from PIL import ImageColor

import os
path = "frontend/src/favicon.ico"
if os.path.exists(path):
    img = Image.open(path)
    img = img.convert("RGBA")
    datas = img.getdata()
    
    # We will assume the top-left pixel is the background color. 
    bg_color = datas[0]
    
    new_data = []
    for item in datas:
        if abs(item[0] - bg_color[0]) < 10 and abs(item[1] - bg_color[1]) < 10 and abs(item[2] - bg_color[2]) < 10:
            new_data.append((255, 255, 255, 0)) # transparent
        else:
            new_data.append(item)
            
    img.putdata(new_data)
    
    os.makedirs("frontend/src/assets", exist_ok=True)
    out_path = "frontend/src/assets/logo.png"
    img.save(out_path, "PNG")
    print(f"Saved transparent logo to {out_path}")
    
    # Save a transparent version of the favicon too
    img.save(path, format="ICO")
    print(f"Overwrote {path}")
else:
    print(f"File not found: {path}")
