<template>
    <div class="login">
        <div class="login-container">
            <LoginPanel />

        </div>
    </div>
</template>
<script setup lang="ts">
import LoginPanel from './c-cpns/login-panel.vue';
</script>

<style lang="less" scoped>
.login {
    height: 100%;
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #f0f4fe;
    background: url('../../../src/assets/img/outer-space-photo.jpg');
}

.login-container {}
</style>