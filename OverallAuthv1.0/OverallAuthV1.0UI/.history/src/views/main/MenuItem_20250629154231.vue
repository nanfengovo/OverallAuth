<!-- MenuItem.vue 递归组件 -->
<template>
    <!-- 无子菜单 -->
    <el-menu-item v-if="!item.children?.length" :index="item.url" :route="{ path: item.url }">
        <el-icon>
            <component :is="item.icon" />
        </el-icon>
        <span>{{ item.describe }}</span>
    </el-menu-item>

    <!-- 有子菜单 -->
    <el-sub-menu v-else :index="item.url">
        <template #title>
            <el-icon>
                <component :is="item.icon" />
            </el-icon>
            <span>{{ item.name }}</span>
        </template>
        <menu-item v-for="child in item.children" :key="child.id" :item="child" />
    </el-sub-menu>
</template>

<script setup lang="ts">
import { defineProps } from 'vue'

defineProps({
    item: {
        type: Object,
        required: true
    }
})
</script>