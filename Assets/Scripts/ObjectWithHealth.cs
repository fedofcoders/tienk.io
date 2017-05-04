﻿//
//  Copyright (c) 2017  FederationOfCoders.org
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;

namespace Tienkio {
    public class ObjectWithHealth : MonoBehaviour {
        public Transform healthBar;
        public float health;
        protected float lastHealth;
        public float maxHealth, healthRegen, extraRegenTimeout, extraRegen;

        protected float lastUpdate, nextExtraRegen;

        Vector3 originalPosition;
        Vector3 originalScale;

        protected virtual void Start() {
            originalPosition = healthBar.localPosition;
            originalScale = healthBar.localScale;
        }

        protected virtual void FixedUpdate() {
            if (lastHealth != health) {
                ResizeBar();
                if (IsExtraRegenEnabled()) nextExtraRegen = Time.time + extraRegenTimeout;
                lastHealth = health;
            }

            if (IsRegenEnabled() || IsExtraRegenEnabled()) {
                float now = Time.time;
                float sinceLastUpdate = now - lastUpdate;
                bool isExtraRegen = IsExtraRegenEnabled() && now >= nextExtraRegen;

                if (health < maxHealth) {
                    float healthPerSecond = isExtraRegen ? extraRegen : healthRegen;
                    float regen = Mathf.Min(maxHealth * healthPerSecond * sinceLastUpdate, maxHealth - health);
                    health = lastHealth += regen;
                    ResizeBar();
                } else if (IsExtraRegenEnabled()) {
                    nextExtraRegen = Time.time + extraRegenTimeout;
                }

                lastUpdate = now;
            }
        }

        void ResizeBar() {
            float width = health * (originalScale.x / maxHealth);
            healthBar.localScale = new Vector3(width, originalScale.y);
            healthBar.localPosition = new Vector3(width / 2f - originalScale.x / 2f, originalPosition.y);
        }

        bool IsRegenEnabled() {
            return healthRegen > 0;
        }

        bool IsExtraRegenEnabled() {
            return extraRegenTimeout > 0 && extraRegen > 0;
        }
    }
}
