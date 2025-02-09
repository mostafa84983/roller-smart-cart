# 🚀 Team Branching Guidelines

## 🔴 DO NOT PUSH DIRECTLY TO `main` OR `dev` 🚫
All code changes must go through **`feature`** branches and be merged into **`base`**.

---

## 📂 Branching Structure
We follow a structured branching strategy to organize our work across teams.

### 🌳 **Main Branches**
- `main` – **Stable, production-ready code**. 🔒 Protected.
- `dev` – **Integration branch for testing before merging into `main`**. 🔒 Protected.

### 🛠 **`base` Branches**

#### 🛠 **Web Team `base` Branches**
The web team has specific `base` branches for different parts of the application:
- **`web/backend`** – Backend development
- **`web/front-global`** – Global frontend components
- **`web/front-cart`** – Cart-specific frontend components

Feature branches should be created under the relevant `base` branch:
```
web/backend/authentication
web/backend/order-processing
web/front-global/navbar-ui
web/front-cart/cart-ui
```

#### 🛠 **Other Team `base` Branches**
- **`devops`** – DevOps (CI/CD, Infrastructure, Automation)
- **`ml`** – Machine Learning (Models, Training, Inference)
- **`embedded`** – Embedded (Raspberry Pi, Sensors, Hardware)

Feature branches should be created under the relevant `base` branch:
```
devops/aws-setup
devops/ci-pipeline
ml/training-v2
ml/object-detection
embedded/raspberry-pi-setup
embedded/sensor-integration
```

---

## 📜 Workflow Rules

1️⃣ **Ensure you are on the appropriate `base` branch before creating a feature branch:**
```sh
git checkout web/backend
git pull origin web/backend
```

2️⃣ **Create a feature branch from the `base` branch:**
```sh
git checkout -b web/backend/authentication
git push origin web/backend/authentication
```

3️⃣ **Work on your changes and commit regularly.**
```sh
git add .
git commit -m "Implemented authentication API"
```

4️⃣ **Once tested and stable, merge to the `base` branch.**
```sh
git checkout web/backend
git merge web/backend/authentication
git push origin web/backend
```

5️⃣ **Delete the merged feature branch:**
```sh
git branch -d web/backend/authentication  # Delete locally
git push origin --delete web/backend/authentication  # Delete remotely
```

6️⃣ **At the integration phase, we will merge all `base` branches (`web/backend`, `web/front-global`, `web/front-cart`, `devops`, `ml`, and `embedded`) into `dev`.**

7️⃣ **Ready code will be merged for production `dev` → `main`.**
- Code ready for deployment and agreed upon from all team members will be merged to `main`.

---

## 🔒 Rules & Best Practices

✅ **Always use feature branches.**  
✅ **Follow naming conventions (`base/feature-name`).**  
✅ **Ensure your changes are tested before merging.**  
✅ **Merge feature branches into `base` branches *not* `dev`.**  
✅ **Delete old branches after merging with your `base`.**  

❌ **DO NOT push directly to `main` or `dev`.**  
❌ **DO NOT merge to `dev` or `main` without team agreement.**  
❌ **DO NOT leave stale branches unmerged.**  

