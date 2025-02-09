# 🚀 Team Branching Guidelines

## 🔴 DO NOT PUSH DIRECTLY TO `main` OR `dev` 🚫
All code changes must go through **feature branches** and be merged via **Pull Requests (PRs)**.

---

## 📂 Branching Structure
We follow a structured branching strategy to organize our work across teams.

### 🌳 **Main Branches**
- `main` – **Stable, production-ready code**. 🔒 Protected.
- `dev` – **Integration branch for testing before merging into `main`**. 🔒 Protected.

### 🛠 **Team Branches**
Each team works in its own namespace:
- **`web/*`** – Web team (Frontend, Backend, API, UI/UX)
- **`devops/*`** – DevOps team (CI/CD, Infrastructure, Automation)
- **`ml/*`** – Machine Learning team (Models, Training, Inference)
- **`embedded/*`** – Embedded team (Raspberry Pi, Sensors, Hardware)

Example branches:
```
web/frontend/cart-ui
web/backend/api-refactor
ml/model/training-v2
devops/infrastructure/aws-setup
embedded/raspberry-pi/setup
```

---

## 📜 Workflow Rules
1️⃣ **Create a feature branch** from `dev`:
```sh
git checkout -b web/frontend/cart-ui
git push origin web/frontend/cart-ui
```

2️⃣ **Work on your changes and commit regularly.**
```sh
git add .
git commit -m "Added cart UI component"
```

3️⃣ **Create a Pull Request (PR) to `dev`.**
- Ensure CI/CD checks pass.

4️⃣ **Once tested and stable, merge to `dev`.**
- `dev` is tested before merging to `main`.

5️⃣ **Only maintainers merge `dev` → `main`.**

---

## 🔒 Rules & Best Practices

✅ **Always use feature branches.**  
✅ **Ensure your changes are tested before merging.**  
✅ **Delete old branches after merging.**  
✅ **Follow naming conventions (`team/feature-name`).**  

❌ **DO NOT push directly to `main` or `dev`.**  
❌ **DO NOT merge to `dev` or `main` without team agreement.**  
❌ **DO NOT leave stale branches unmerged.**  


