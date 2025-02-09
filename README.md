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
Each team has its own dedicated branch to work in before merging to `dev`:
- **`web`** – Web team (Frontend, Backend, API, UI/UX)
- **`devops`** – DevOps team (CI/CD, Infrastructure, Automation)
- **`ml`** – Machine Learning team (Models, Training, Inference)
- **`embedded`** – Embedded team (Raspberry Pi, Sensors, Hardware)

Each team creates feature branches under their respective team branch:
```
web/cart-ui
web/backend-api
ml/training-v2
devops/aws-setup
embedded/raspberry-pi-setup
```

---

## 📜 Workflow Rules

1️⃣ **Ensure you are on your team's branch (e.g., `web`, `ml`):**
```sh
git checkout web
git pull origin web
```

2️⃣ **Create a feature branch from your team's branch:**
```sh
git checkout -b web/cart-ui
git push origin web/cart-ui
```

3️⃣ **Work on your changes and commit regularly.**
```sh
git add .
git commit -m "Added cart UI component"
```

4️⃣ **Create a Pull Request (PR) to your team's branch (`web`, `ml`, etc.).**
- Ensure CI/CD checks pass.

5️⃣ **Once tested and stable, merge to your team's branch.**

6️⃣ **At the integration phase, we will merge `web`, `ml`, `devops`, and `embedded` into `dev`.**

7️⃣ **Ready code will be merged for production `dev` → `main`..**
- Code ready for deployment and agreed upon from all team members will be merged to `main`.

---

## 🔒 Rules & Best Practices

✅ **Always use feature branches.**  
✅ **Ensure your changes are tested before merging.**  
✅ **Merge feature branches into team branches before `dev`.**  
✅ **Follow naming conventions (`team/feature-name`).**  
✅ **Delete old branches after merging.**  

❌ **DO NOT push directly to `main` or `dev`.**  
❌ **DO NOT merge to `dev` or `main` without team agreement.**  
❌ **DO NOT leave stale branches unmerged.**  

