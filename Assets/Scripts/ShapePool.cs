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
    [System.Serializable]
    public class ShapeSpawnerPool {
    	public float chance;
    	public PoolManager pool;
    }

    public class ShapePool : Singleton<ShapePool> {
        public ShapeSpawnerPool[] shapePools;
        public Rect fieldBoundary;
        public int shapesCount;

        void Start() {
            for (int i = 0; i < shapesCount; i++) SpawnShape();
        }

        public void SpawnShape() {
            PoolManager pool = SelectPool();
            if (pool != null) {
                Vector2 position = new Vector2(
                    Random.Range(fieldBoundary.x, fieldBoundary.width),
                    Random.Range(fieldBoundary.y, fieldBoundary.height)
                );
                pool.GetFromPool(position, Quaternion.identity);
            }
        }

        public PoolManager SelectPool() {
            foreach (ShapeSpawnerPool shapeSpawnerPool in shapePools) {
                float random = Random.value;
                if (random <= shapeSpawnerPool.chance) return shapeSpawnerPool.pool;
            }
            return null;
        }

        public void DestroyShape(PoolObject shape) {
            shape.PutIntoPool();
        }
    }
}
