<template>
    <div class="login-panel">
        <h1 class="title">权限管理系统</h1>
        <div class="tab">tabs</div>
        <div class="controls">
            <el-checkbox v-model="isRemPwd" label="记住密码" />
            <el-link type="primary">忘记密码</el-link>
        </div>
        <div class="login-action">登录</div>
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const isRemPwd = ref(false);
</script>

<style lang="less" scoped>
.login-panel {
    width: 330px;
}
</style>