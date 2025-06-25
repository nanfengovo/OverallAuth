<template>
    <div class="foot">
        <P>Powered by Vue3 + Vite + TypeScript</P>
        <div class="copyright">
            <p>Copyright © 2025</p>
            <a href="https://github.com/nanfengovo"> nanfengovo </a>
        </div>

    </div>
</template>
<script setup lang="ts">
</script>

<style lang="less" scoped>
.foot {}

.copyright {
    display: flex;
    justify-content: center;
    align-items: center;

    a {
        margin-left: 10px;
        color: #fff;
        text-decoration: none;
    }
}

p {
    text-align: center;
    margin-top: 5px;
}
</style>