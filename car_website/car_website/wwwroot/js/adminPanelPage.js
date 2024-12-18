﻿const fuelName = (id) => ["Газ", "Газ/Бензин", "Бензин", "Дизель", "Гібрид", "Електро"][id - 1];
const transmissionName = (id) => id == 1 ? "Механічна" : "Автомат";
const drivelineName = (id) => ["Передній", "Задній", "Повний"][id - 1];
const formatNumberWithThousandsSeparator = (number) => number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
const svgCodes = {
    edit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_217_311)"><path d="M24 0H0V24H24V0Z" fill="white" fill-opacity="0.01"/><path d="M21 13V20C21 20.5523 20.5523 21 20 21H4C3.44771 21 3 20.5523 3 20V4C3 3.44771 3.44771 3 4 3H11" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M7 13.36V17H10.6586L21 6.65405L17.3475 3L7 13.36Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/></g><defs><clipPath id="clip0_217_311"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    delete: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_217_304)"><path d="M4.5 5V22H19.5V5H4.5Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M10 10V16.5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M14 10V16.5" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M2 5H22" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M8 5L9.6445 2H14.3885L16 5H8Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/></g><defs><clipPath id="clip0_217_304"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    submit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_235_308)"><path d="M5 12L10 17L20 7" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_235_308"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    cancel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M13.4143 12.0002L18.7072 6.70725C19.0982 6.31625 19.0982 5.68425 18.7072 5.29325C18.3162 4.90225 17.6843 4.90225 17.2933 5.29325L12.0002 10.5862L6.70725 5.29325C6.31625 4.90225 5.68425 4.90225 5.29325 5.29325C4.90225 5.68425 4.90225 6.31625 5.29325 6.70725L10.5862 12.0002L5.29325 17.2933C4.90225 17.6843 4.90225 18.3162 5.29325 18.7072C5.48825 18.9022 5.74425 19.0002 6.00025 19.0002C6.25625 19.0002 6.51225 18.9022 6.70725 18.7072L12.0002 13.4143L17.2933 18.7072C17.4883 18.9022 17.7442 19.0002 18.0002 19.0002C18.2562 19.0002 18.5122 18.9022 18.7072 18.7072C19.0982 18.3162 19.0982 17.6843 18.7072 17.2933L13.4143 12.0002Z" fill="currentColor"/></svg>`,
    add: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_223_315)"><path d="M19.5 3H4.5C3.67157 3 3 3.67157 3 4.5V19.5C3 20.3284 3.67157 21 4.5 21H19.5C20.3284 21 21 20.3284 21 19.5V4.5C21 3.67157 20.3284 3 19.5 3Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M12 8V16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M8 12H16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_223_315"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    driveline: `<svg version="1.0" xmlns="http://www.w3.org/2000/svg"width="24" height="24" viewBox="0 0 512.000000 512.000000"preserveAspectRatio="xMidYMid meet"><g transform="translate(0.000000,512.000000) scale(0.100000,-0.100000)" fill="currentColor" stroke="none"><path d="M950 5007 c-50 -16 -136 -103 -149 -152 -7 -26 -11 -208 -11 -531 l0 -490 28 -55 c33 -66 93 -113 162 -128 80 -17 356 -14 419 5 64 19 127 73 155 134 18 39 21 70 24 243 l4 197 143 0 142 0 6 -58 c5 -66 32 -124 74 -164 36 -35 109 -68 151 -68 18 0 40 -8 52 -20 19 -19 20 -33 20 -252 0 -268 5 -285 83 -354 50 -44 102 -64 166 -64 l41 0 0 -1180 0 -1180 -439 0 -439 0 -4 198 c-3 172 -6 203 -24 242 -28 61 -91 115 -155 134 -72 22 -364 22 -435 1 -66 -19 -121 -67 -150 -129 l-24 -51 0 -490 c0 -323 4 -504 11 -530 14 -53 99 -136 155 -153 59 -17 406 -16 457 2 57 21 100 58 132 117 29 54 29 55 33 257 l4 202 978 0 978 0 4 -202 c4 -202 4 -203 33 -257 32 -59 75 -96 132 -117 58 -20 404 -19 466 2 56 19 131 97 147 152 6 23 10 225 10 527 l0 490 -24 51 c-29 62 -84 110 -150 129 -71 21 -363 21 -435 -1 -64 -19 -127 -73 -155 -134 -18 -39 -21 -70 -24 -242 l-4 -198 -439 0 -439 0 0 1180 0 1180 43 0 c97 0 188 58 228 145 16 35 19 68 19 273 0 219 1 233 20 252 12 12 34 20 53 20 41 0 114 33 150 68 42 40 69 98 74 164 l6 58 142 0 143 0 4 -197 c3 -173 6 -204 24 -243 28 -61 91 -115 155 -134 63 -19 339 -22 419 -5 69 15 129 62 163 128 l27 55 0 490 c0 303 -4 505 -10 528 -16 55 -91 133 -147 152 -62 21 -408 22 -466 2 -57 -21 -100 -58 -132 -117 -29 -54 -29 -55 -33 -256 l-4 -203 -144 0 -144 0 0 38 c0 96 -50 182 -132 227 l-53 30 -505 0 c-502 0 -505 0 -546 -22 -87 -47 -132 -115 -141 -214 l-6 -59 -142 0 -143 0 -4 203 c-4 201 -4 202 -33 256 -32 59 -75 96 -132 117 -48 17 -411 17 -463 1z m420 -206 c6 -13 10 -180 10 -474 0 -429 -1 -455 -19 -471 -16 -14 -40 -16 -187 -14 -168 3 -168 3 -181 28 -10 19 -12 126 -11 478 3 390 5 454 18 462 8 5 92 10 187 10 160 0 173 -1 183 -19z m2750 9 c13 -8 15 -72 18 -462 1 -352 -1 -459 -11 -478 -13 -25 -13 -25 -181 -28 -147 -2 -171 0 -187 14 -18 16 -19 42 -19 471 0 294 4 461 10 474 10 18 23 19 183 19 95 0 179 -5 187 -10z m-1089 -290 c18 -10 19 -23 19 -184 0 -196 1 -192 -77 -205 -95 -15 -155 -60 -194 -146 -16 -35 -19 -69 -19 -268 0 -138 -4 -236 -10 -248 -10 -18 -23 -19 -190 -19 -167 0 -180 1 -190 19 -6 12 -10 110 -10 248 0 199 -3 233 -19 268 -39 86 -99 131 -194 146 -78 13 -77 9 -77 205 0 154 2 174 18 183 24 14 916 15 943 1z m-1669 -3257 c17 -15 18 -43 18 -470 0 -294 -4 -461 -10 -474 -10 -18 -23 -19 -183 -19 -95 0 -179 5 -187 10 -13 8 -15 72 -15 473 0 379 3 467 14 480 20 25 335 25 363 0z m2765 -13 c10 -19 12 -126 11 -478 -3 -390 -5 -454 -18 -462 -8 -5 -92 -10 -187 -10 -160 0 -173 1 -183 19 -6 13 -10 179 -10 472 0 409 2 454 17 471 15 17 32 18 187 16 l170 -3 13 -25z"/></g></svg>`,
    car: `<svg width="24" height="24" viewBox="0 0 30 30" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M24.9997 14.6876H4.9997C4.7122 14.6876 4.4497 14.5626 4.2747 14.3376C4.0997 14.1251 4.0247 13.8251 4.0872 13.5501L5.4997 6.8001C5.9622 4.6126 6.8997 2.6001 10.6122 2.6001H19.3997C23.1122 2.6001 24.0497 4.6251 24.5122 6.8001L25.9247 13.5626C25.9872 13.8376 25.9122 14.1251 25.7372 14.3501C25.5497 14.5626 25.2872 14.6876 24.9997 14.6876ZM6.1497 12.8126H23.8372L22.6622 7.1876C22.3122 5.5501 21.8997 4.4751 19.3872 4.4751H10.6122C8.0997 4.4751 7.6872 5.5501 7.3372 7.1876L6.1497 12.8126Z" fill="currentColor"/><path d="M24.9495 28.4376H22.5995C20.5745 28.4376 20.187 27.2751 19.937 26.5126L19.687 25.7626C19.362 24.8126 19.3245 24.6876 18.1995 24.6876H11.7995C10.6745 24.6876 10.5995 24.9001 10.312 25.7626L10.062 26.5126C9.7995 27.2876 9.4245 28.4376 7.3995 28.4376H5.0495C4.062 28.4376 3.112 28.0251 2.4495 27.3001C1.7995 26.5876 1.487 25.6376 1.5745 24.6876L2.2745 17.0751C2.462 15.0126 3.012 12.8126 7.0245 12.8126H22.9745C26.987 12.8126 27.5245 15.0126 27.7245 17.0751L28.4245 24.6876C28.512 25.6376 28.1995 26.5876 27.5495 27.3001C26.887 28.0251 25.937 28.4376 24.9495 28.4376ZM11.7995 22.8126H18.1995C20.4745 22.8126 21.012 23.8251 21.462 25.1501L21.7245 25.9251C21.937 26.5626 21.937 26.5751 22.612 26.5751H24.962C25.4245 26.5751 25.862 26.3876 26.1745 26.0501C26.4745 25.7251 26.612 25.3126 26.5745 24.8751L25.8745 17.2626C25.712 15.5751 25.512 14.7001 22.9995 14.7001H7.0245C4.4995 14.7001 4.2995 15.5751 4.1495 17.2626L3.4495 24.8751C3.412 25.3126 3.5495 25.7251 3.8495 26.0501C4.1495 26.3876 4.5995 26.5751 5.062 26.5751H7.412C8.087 26.5751 8.087 26.5626 8.2995 25.9376L8.5495 25.1751C8.862 24.1751 9.3245 22.8126 11.7995 22.8126Z" fill="currentColor"/><path d="M4.99951 10.9376H3.74951C3.23701 10.9376 2.81201 10.5126 2.81201 10.0001C2.81201 9.48759 3.23701 9.06259 3.74951 9.06259H4.99951C5.51201 9.06259 5.93701 9.48759 5.93701 10.0001C5.93701 10.5126 5.51201 10.9376 4.99951 10.9376Z" fill="currentColor"/><path d="M26.2495 10.9376H24.9995C24.487 10.9376 24.062 10.5126 24.062 10.0001C24.062 9.48759 24.487 9.06259 24.9995 9.06259H26.2495C26.762 9.06259 27.187 9.48759 27.187 10.0001C27.187 10.5126 26.762 10.9376 26.2495 10.9376Z" fill="currentColor"/><path d="M14.9995 7.18759C14.487 7.18759 14.062 6.76259 14.062 6.25009V3.75009C14.062 3.23759 14.487 2.81259 14.9995 2.81259C15.512 2.81259 15.937 3.23759 15.937 3.75009V6.25009C15.937 6.76259 15.512 7.18759 14.9995 7.18759Z" fill="currentColor"/><path d="M16.8745 7.18759H13.1245C12.612 7.18759 12.187 6.76259 12.187 6.25009C12.187 5.73759 12.612 5.31259 13.1245 5.31259H16.8745C17.387 5.31259 17.812 5.73759 17.812 6.25009C17.812 6.76259 17.387 7.18759 16.8745 7.18759Z" fill="currentColor"/><path d="M11.2495 19.6876H7.49951C6.98701 19.6876 6.56201 19.2626 6.56201 18.7501C6.56201 18.2376 6.98701 17.8126 7.49951 17.8126H11.2495C11.762 17.8126 12.187 18.2376 12.187 18.7501C12.187 19.2626 11.762 19.6876 11.2495 19.6876Z" fill="currentColor"/><path d="M22.4995 19.6876H18.7495C18.237 19.6876 17.812 19.2626 17.812 18.7501C17.812 18.2376 18.237 17.8126 18.7495 17.8126H22.4995C23.012 17.8126 23.437 18.2376 23.437 18.7501C23.437 19.2626 23.012 19.6876 22.4995 19.6876Z" fill="currentColor"/></svg>`,
    fuel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M17.5 22.75H3.5C3.09 22.75 2.75 22.41 2.75 22V5.00001C2.75 2.76001 4.26 1.25001 6.5 1.25001H14.5C16.74 1.25001 18.25 2.76001 18.25 5.00001V22C18.25 22.41 17.91 22.75 17.5 22.75ZM4.25 21.25H16.75V5.00001C16.75 3.59001 15.91 2.75001 14.5 2.75001H6.5C5.09 2.75001 4.25 3.59001 4.25 5.00001V21.25Z" fill="currentColor"/><path d="M18.9999 22.75H1.99994C1.58994 22.75 1.24994 22.41 1.24994 22C1.24994 21.59 1.58994 21.25 1.99994 21.25H18.9999C19.4099 21.25 19.7499 21.59 19.7499 22C19.7499 22.41 19.4099 22.75 18.9999 22.75Z" fill="currentColor"/><path d="M12.6102 10.75H8.38023C6.75023 10.75 5.74023 9.74 5.74023 8.11V6.88C5.74023 5.25 6.75023 4.24001 8.38023 4.24001H12.6102C14.2402 4.24001 15.2502 5.25 15.2502 6.88V8.11C15.2502 9.74 14.2402 10.75 12.6102 10.75ZM8.39023 5.75C7.58023 5.75 7.25023 6.08 7.25023 6.89V8.12C7.25023 8.93 7.58023 9.25999 8.39023 9.25999H12.6202C13.4302 9.25999 13.7602 8.93 13.7602 8.12V6.89C13.7602 6.08 13.4302 5.75 12.6202 5.75H8.39023Z" fill="currentColor"/><path d="M9.49994 13.75H6.49994C6.08994 13.75 5.74994 13.41 5.74994 13C5.74994 12.59 6.08994 12.25 6.49994 12.25H9.49994C9.90994 12.25 10.2499 12.59 10.2499 13C10.2499 13.41 9.90994 13.75 9.49994 13.75Z" fill="currentColor"/><path d="M17.5 16.76C17.09 16.76 16.75 16.43 16.75 16.01C16.75 15.6 17.08 15.26 17.5 15.26L21.25 15.25V10.46L19.66 9.66998C19.29 9.47998 19.14 9.02998 19.32 8.65998C19.51 8.28998 19.96 8.13999 20.33 8.31999L22.33 9.31999C22.58 9.44999 22.74 9.70998 22.74 9.98998V15.99C22.74 16.4 22.41 16.74 21.99 16.74L17.5 16.76Z" fill="currentColor"/></svg>`,
    transmission: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M12 15.75C9.93 15.75 8.25 14.07 8.25 12C8.25 9.93 9.93 8.25 12 8.25C14.07 8.25 15.75 9.93 15.75 12C15.75 14.07 14.07 15.75 12 15.75ZM12 9.75C10.76 9.75 9.75 10.76 9.75 12C9.75 13.24 10.76 14.25 12 14.25C13.24 14.25 14.25 13.24 14.25 12C14.25 10.76 13.24 9.75 12 9.75Z" fill="currentColor"/><path d="M15.21 22.19C15 22.19 14.79 22.16 14.58 22.11C13.96 21.94 13.44 21.55 13.11 21L12.99 20.8C12.4 19.78 11.59 19.78 11 20.8L10.89 20.99C10.56 21.55 10.04 21.95 9.42 22.11C8.79 22.28 8.14 22.19 7.59 21.86L5.87 20.87C5.26 20.52 4.82 19.95 4.63 19.26C4.45 18.57 4.54 17.86 4.89 17.25C5.18 16.74 5.26 16.28 5.09 15.99C4.92 15.7 4.49 15.53 3.9 15.53C2.44 15.53 1.25 14.34 1.25 12.88V11.12C1.25 9.66 2.44 8.47 3.9 8.47C4.49 8.47 4.92 8.3 5.09 8.01C5.26 7.72 5.19 7.26 4.89 6.75C4.54 6.14 4.45 5.42 4.63 4.74C4.81 4.05 5.25 3.48 5.87 3.13L7.6 2.14C8.73 1.47 10.22 1.86 10.9 3.01L11.02 3.21C11.61 4.23 12.42 4.23 13.01 3.21L13.12 3.02C13.8 1.86 15.29 1.47 16.43 2.15L18.15 3.14C18.76 3.49 19.2 4.06 19.39 4.75C19.57 5.44 19.48 6.15 19.13 6.76C18.84 7.27 18.76 7.73 18.93 8.02C19.1 8.31 19.53 8.48 20.12 8.48C21.58 8.48 22.77 9.67 22.77 11.13V12.89C22.77 14.35 21.58 15.54 20.12 15.54C19.53 15.54 19.1 15.71 18.93 16C18.76 16.29 18.83 16.75 19.13 17.26C19.48 17.87 19.58 18.59 19.39 19.27C19.21 19.96 18.77 20.53 18.15 20.88L16.42 21.87C16.04 22.08 15.63 22.19 15.21 22.19ZM12 18.49C12.89 18.49 13.72 19.05 14.29 20.04L14.4 20.23C14.52 20.44 14.72 20.59 14.96 20.65C15.2 20.71 15.44 20.68 15.64 20.56L17.37 19.56C17.63 19.41 17.83 19.16 17.91 18.86C17.99 18.56 17.95 18.25 17.8 17.99C17.23 17.01 17.16 16 17.6 15.23C18.04 14.46 18.95 14.02 20.09 14.02C20.73 14.02 21.24 13.51 21.24 12.87V11.11C21.24 10.48 20.73 9.96 20.09 9.96C18.95 9.96 18.04 9.52 17.6 8.75C17.16 7.98 17.23 6.97 17.8 5.99C17.95 5.73 17.99 5.42 17.91 5.12C17.83 4.82 17.64 4.58 17.38 4.42L15.65 3.43C15.22 3.17 14.65 3.32 14.39 3.76L14.28 3.95C13.71 4.94 12.88 5.5 11.99 5.5C11.1 5.5 10.27 4.94 9.7 3.95L9.59 3.75C9.34 3.33 8.78 3.18 8.35 3.43L6.62 4.43C6.36 4.58 6.16 4.83 6.08 5.13C6 5.43 6.04 5.74 6.19 6C6.76 6.98 6.83 7.99 6.39 8.76C5.95 9.53 5.04 9.97 3.9 9.97C3.26 9.97 2.75 10.48 2.75 11.12V12.88C2.75 13.51 3.26 14.03 3.9 14.03C5.04 14.03 5.95 14.47 6.39 15.24C6.83 16.01 6.76 17.02 6.19 18C6.04 18.26 6 18.57 6.08 18.87C6.16 19.17 6.35 19.41 6.61 19.57L8.34 20.56C8.55 20.69 8.8 20.72 9.03 20.66C9.27 20.6 9.47 20.44 9.6 20.23L9.71 20.04C10.28 19.06 11.11 18.49 12 18.49Z" fill="currentColor"/></svg>`,
    race: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_14_212)"><path d="M15.1486 9.3893C15.1486 9.3893 13.534 13.9404 12.7667 14.735C11.9994 15.5296 10.7333 15.5517 9.93872 14.7844C9.14412 14.0171 9.12202 12.7509 9.88932 11.9564C10.6566 11.1618 15.1486 9.3893 15.1486 9.3893Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M19.4246 19.4246C21.3248 17.5245 22.5 14.8995 22.5 12C22.5 6.201 17.799 1.5 12 1.5C6.201 1.5 1.5 6.201 1.5 12C1.5 14.8995 2.67525 17.5245 4.57538 19.4246" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M12 2V4" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M19.4227 5.57105L17.8684 6.82965" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M21.2613 13.6164L19.3126 13.1665" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M2.73877 13.6164L4.68751 13.1665" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M4.57733 5.57105L6.13162 6.82965" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_14_212"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    cross: `<svg width="24" height="24" viewBox="0 0 16 16" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M4.11 2.697L2.698 4.11 6.586 8l-3.89 3.89 1.415 1.413L8 9.414l3.89 3.89 1.413-1.415L9.414 8l3.89-3.89-1.415-1.413L8 6.586l-3.89-3.89z" fill="#000"></path></svg>`
};
const openAdminLeft = document.getElementById('open-admin-left'),
adminLeft = document.querySelector('.admin-left'),
radioButtons = document.querySelectorAll('input[name="action"]'),
container = document.getElementById('container');
let buyRequestsPage = 1, buyRequestsCache;
let botCarsPage = 1, botCarsCache;
let waitingCarsPage = 1, waitingCarsCache;
let usersPage = 1, usersCache;
let brandsPage = 1, brandsCache;
let expressCarsPage = 1, expressCarsCache;
let ordersCache;
let modelsCache = {};
let selectedBrand;
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");
let selectedRadio;
window.addEventListener('load', function () {
    radioButtons.forEach(function (radio) {
        if (radio.checked) {
            selectedRadio = radio;
            updateInfo(radio);
        }
    });
});
openAdminLeft.addEventListener('click', () => adminLeft.classList.toggle("open"));

radioButtons.forEach(function (radio) {
    radio.addEventListener('change', handleRadioChange);
});
function handleRadioChange(event) {
    selectedRadio = event.target;
    updateInfo(event.target);
}
function updateInfo(target,page=1) {
    container.innerHTML = "";
    if (target.id == "users") {
        usersPage = page;
        if (usersCache == null || usersCache.page != page) {
            container.innerHTML = "<p class='user-downloading'>Завантажується</p>";
            getUsers();
        }
        else
            showData(usersCache);
    }
    else if (target.id == "buyRequests") {
        if (buyRequestsCache == null)
            getBuyRequests();
        else
            showData(buyRequestsCache);
    }
    else if (target.id == "waitingCars") {
        if (waitingCarsCache == null)
            getWaitingCars();
        else
            showData(waitingCarsCache);
    }
    else if (target.id == "brands") {
        if (brandsCache == null)
            getBrands();
        else
            showData(brandsCache);
    }
    else if (target.id == "expressSales") {
        if (expressCarsCache == null)
            getExpressSaleCars();
        else {
            showData(expressCarsCache);
        }
    }
    else if (target.id == "botCars") {
        if (botCarsCache == null)
            getBotCars();
        else {
            showData(botCarsCache);
        }
    }
    else if (target.id == "currencyRate") {
        showCurrencyEditor();
    }
    else if (target.id == "orders") {
        if (ordersCache == null)
            getOrders();
        else {
            showData(ordersCache);
        }
    }
    else if (target.id == "dev") {
        showDevPanel();
    }
}
function deleteRequest(sender) {
    if (confirm(`Видалити цей запрос на ${sender.getAttribute('car')}?`)) {
        fetch(`/api/v1/cars/deleteBuyRequest?id=${sender.getAttribute('id') }`, {
            method: 'DELETE',
        })
            .then(response => response.json())
            .then(data => {
                console.log(data)
                if (data != null && data.status == true)
                    sender.parentElement.remove();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function showData(data) {
    if (data == null || data.status == false || data.success == false)
        container.innerHTML = `<h3 class="warning-text">Помилка при отриманні даних</h3>`;
    else {
        if (data.pages)
        updatePagesButtons(data.pages)
        console.log(usersPage)
        if (data.type == "Users") {
            container.innerHTML = "";
            if (data.users.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нікого немає</h3>`;
            data.users.forEach(user => {
                container.innerHTML += `<div class="user_container-element">
                <a href="/User/Detail/${user.id}">${user.name} ${user.surname}${user.emailConfirmed ? '<span class="confirmed"></span>' : ''}</a>
                <p>+${user.phoneNumber}</p>
                <p>${user.email}</p>
                ${user.role != 0 ? user.role == 1 ? '<span class="role">Адмін</span>' : user.role == 2 ? '<span class="role">Розробник</span>' : '<span class="role">Модератор</span>' : ''}
            </div>`;
            });
        }
        else if (data.type == "WaitingCars") {
            console.log(data);
            container.innerHTML = "";
            if (data.cars.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            data.cars.forEach(car => {
                container.innerHTML += `<a class="car mainPageCar" href="/Car/WaitingCarDetail/${car.id}">
                                  <p class="car_name">${car.car.brand} ${car.car.model} ${car.car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img"> <div class="car_container-img-landscape"><img  alt="${car.car.brand} ${car.car.model} ${car.car.year}" src="${car.car.previewURL}" /></div></div>
                                    <div class="car_container-info">
                                    <p class="car_container-info-name">${car.car.brand} ${car.car.model} ${car.car.year}</p>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.race}</span>${car.car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.fuel}</span>${fuelName(car.car.fuel)}, ${car.car.engineCapacity} ${car.car.fuel == 6 ? "кВт·год." : "л."}</p>
                                                    ${car.car.vin == null ? `` : `<p class="car_container-info-parameters-column-text vin"><span>${svgCodes.car}</span>${car.car.vin}</p>`}
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.transmission}</span>${transmissionName(car.car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.driveline}</span>${drivelineName(car.car.driveline)}</p>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.car.priceUAH)} грн</p>
                                            </div>
                                  </div>
                                  </a>`;
            });
        }
        else if (data.type == "BotCars") {
            console.log(data);
            container.innerHTML = "";
            if (data.cars.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            data.cars.forEach(car => {
                container.innerHTML += `<a class="car mainPageCar" href="/Car/BotDetail/${car.id}">
                                  <p class="car_name">${car.brand} ${car.model} ${car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img"> <div class="car_container-img-landscape"><img  alt="${car.brand} ${car.model} ${car.year}" src="${car.previewURL}" /></div></div>
                                    <div class="car_container-info">
                                    <p class="car_container-info-name">${car.brand} ${car.model} ${car.year}</p>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.fuel}</span>${fuelName(car.fuel)}, ${car.engineCapacity} ${car.fuel == 6 ? "кВт·год." : "л."}</p>
                                                    </div>
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.transmission}</span>${transmissionName(car.carTransmission)}</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.driveline}</span>${drivelineName(car.driveline)}</p>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="car_container-right">
                                    <div class="car_container-right-price">
                                                <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                                <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                            </div>
                                  </div>
                                  </a>`;
            });
        }
        else if (data.type == "BuyRequests") {
            container.innerHTML = "";
            if (data.requests.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            data.requests.forEach(request => {
                container.innerHTML += `<div class="buy-request">
            <div class="buy-request_buyer">
                <h3>Клієнт:</h3>
                ${request.buyerId ? `<a href="/User/Detail/${request.buyerId}">${request.buyerName}</a>` : `<p class="buy-request-name">${request.buyerName}</p>`}
                <p>+${request.buyerPhone}</p>
                ${request.buyerId ? '' : '<p>Не зареєстрований</p>'}
            </div>
            <div class="buy-request_car">
                <a href="/Car/Detail/${request.carId}">${request.carInfo}</a>
                <img alt="photo" src=${request.carPhotoURL}>
            </div>
            <div class="buy-request_seller">
                <h3>Власник:</h3>
                <a href="/User/Detail/${request.sellerId}">${request.sellerName}</a>
                <p>+${request.sellerPhone}</p>
            </div>
            <button id=${request.id} car="${request.carInfo}" onclick="deleteRequest(this)">Видалити</button>
        </div>`;
            });
        }
        else if (data.type == "Brands") {
            updatePagesButtons(1);
            if (data.brands.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            else {
                container.innerHTML = '<div class="brands-and-models"></div>';
                container.firstElementChild.innerHTML += '<div id="modelsCont" style="display:none"></div><div id="brandsCont"></div>';
                let brandsContainer = document.getElementById('brandsCont');
                let modelsContainer = document.getElementById('modelsCont');

                /*function toggleVisibility() {
                    if (modelsContainer.style.display === 'block' || modelsContainer.style.display === '') {
                        brandsContainer.style.display = 'none';
                    } else {
                        brandsContainer.style.display = 'block';
                    }
                }*/
                // Example of opening modelsCont
                brandsContainer.addEventListener('click', function () {
                    modelsContainer.style.display = 'block';
                    brandsContainer.style.display = 'none';
                   // toggleVisibility();
                });

                // Example of hiding modelsCont
               /* brandsContainer.addEventListener('click', function () {
                    modelsContainer.style.display = 'none';
                    toggleVisibility();
                });*/

                // Initial call to set the correct visibility state
                //toggleVisibility();
                brandsContainer.innerHTML = `<div class="new-model"><input type="text" placeholder="Назва марки" id="new-brand-name"/>
                <button onclick="addBrand(this)"><span>${svgCodes.add}
                </span></button></div>`;
                if (selectedBrand == null || !data.brands.includes(selectedBrand)) selectedBrand = data.brands[0];
                data.brands.forEach(brand => {
                    if (brand != "Інше") {
                        let currBrand = document.createElement('div');
                        currBrand.classList.add('brand');
                        currBrand.innerHTML = `<input type="radio" id="${brand.replace(' ', '_')}"
                        name="brands" value="${brand.replace(' ', '_')}" ${brand == selectedBrand ? 'checked' : ''}>
                        <label for="${brand.replace(' ', '_')}">${brand}</label><div model_buttons>
                        <button brand="${brand.replace(' ', '_')}" class="model_buttons-edit"><span>
                        ${svgCodes.edit}
                        </span></button><button onclick="deleteBrand(this)" brand="${brand.replace(' ', '_')}" class="model_buttons-delete"><span>${svgCodes.delete}
                        </span></button></div>`;
                        if (brand == selectedBrand)
                            getModelsOfMark(brand);
                        brandsContainer.appendChild(currBrand);
                        currBrand.addEventListener('change', (event) => {
                            selectedBrand = event.target.getAttribute('value').replace('_', ' ');
                            getModelsOfMark(event.target.getAttribute('value'));
                        });
                    }
                });
            }
        }
        else if (data.type == "Express") {
            if (data.cars.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            else {
                container.innerHTML = '';
                data.cars.forEach(car => {
                    container.innerHTML += `<div class="car">
                                  <div class="car_container">
                                        <div class="car_container-img"> <div class="car_container-img-landscape"><img style="object-fit:cover; width: 100%;" alt="${car.brand} ${car.model} ${car.year}" src="${car.photosURL[0]}" /></div></div>
                                    <div class="car_container-info">
                                        <a href="/Car/ExpressDetail/${car.id}" style="color: white; font-size: 26px;">${car.brand} ${car.model} ${car.year}</a>
                                    </div>
                                  </div>
                                  <div class="car_container-right">
                                    <div class="car_container-right-price">
                                         <p class="car_container-right-price-USD">${formatNumberWithThousandsSeparator(car.price)} $</p>
                                         <p class="car_container-right-price-UAH">≈ ${formatNumberWithThousandsSeparator(car.priceUAH)} грн</p>
                                    </div>
                                  </div>
                                  </div>
                                </div>`;
                });
            }
        }
        else if (data.type == "Orders"){
            if (data.orders.length == 0)
                container.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
            else {
                container.innerHTML = '';
                data.orders.forEach(order => {
                    container.innerHTML += `<div class='order'>
                    <div class='order-row'><p>Марка: </p><span>${order.brand||'Не обрано'}</span></div>
                    <div class='order-row'><p>Модель: </p><span>${order.model || 'Не обрано'}</span></div>
                    <div class='order-row'><p>Рік: </p><span>${order.year || 'Не обрано'}</span></div>
                    <div class='order-row'><p>Максимальна ціна: </p><span>${order.maxPrice || 'Не обрано'}</span></div>
                    ${order.description?`<div class='order-row'><p>Опис:</p><span>${order.description}</span></div>`:''}
                    <div class='order-row'><p>Ім'я: </p>${order.userId ? `<a href="/User/Detail/${order.userId}">${order.name}</a>`:`<span>${order.name}</span>`}</div>
                    <div class='order-row'><p>Телефон: </p><span>${order.phone}</span></div>
                    </div>`;
                });
            }
        }
    }
}



function putRequest(endpoint) {
    fetch(`/api/v1/main/${endpoint}`, { method:'PUT' })
        .then(response => response.json())
        .then(data => {
            if (data && data.message)
                showNotification(data.message);
            else {
                console.error('Response: ',data);
                if (data && data.code)
                    showNotification(`Помилка (HHTP Code ${data.code})`, true);
                else
                    showNotification('Невідома помилка', true);
            }
        })
        .catch(error => {
            console.error("An error occurred while retrieving data:", error);
            showNotification('Невідома помилка', true);
        });
}
function showDevPanel() {
    container.innerHTML = `<button id="updateCars">Update cars</button><button id="updateUsers">Update users</button><button id="updateOrders">Update orders</button><div><button id="devAction" style="width: 100%;">Dev action</button><p style="color: var(--text-default); margin-top: 3px;" id="action-description"></p></div>`;


    document.getElementById('updateCars').addEventListener('click', () => {
        putRequest("updateCars");
    });
    document.getElementById('updateUsers').addEventListener('click', () => {
        putRequest("updateUsers");
    });
    document.getElementById('updateOrders').addEventListener('click', () => {
        putRequest("updateOrders");
    });
    document.getElementById('devAction').addEventListener('click', () => {
        putRequest("devAction?action=exec");
    });
    fetch('/api/v1/main/devAction?action=ask', { method: 'PUT' })
        .then(response => response.json())
        .then(data => {
            if (data && data.message)
                document.getElementById('action-description').innerHTML = data.message;
            else {
                console.error('Response: ', data);
                if (data && data.code)
                    showNotification(`Помилка (HHTP Code ${data.code})`, true);
                else
                    showNotification('Невідома помилка', true);
            }
        })
        .catch(error => {
            console.error("An error occurred while retrieving data:", error);
            showNotification('Невідома помилка', true);
        });
}
function updatePagesButtons(number) {
    Array.from(pages_buttons_containers).forEach(buttons_container => {
        buttons_container.innerHTML = "";
        if (number > 1) {
            let currentPage = 1;
            if (selectedRadio.id == "users")
                currentPage = usersPage;
            else if (selectedRadio.id == "waitingCars")
                currentPage = waitingCarsPage;
            else if (selectedRadio.id == "buyRequests")
                currentPage = buyRequestsPage;
            else if (selectedRadio.id == "expressSales")
                currentPage = expressCarsPage;
            if (number > 7) {
                if (currentPage >= 5 && number - currentPage > 4) {
                    buttons_container.innerHTML += `<button ${1 === currentPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = currentPage - 1; i <= (number > currentPage + 4 ? currentPage + 2 : number); i++) {
                        buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                    if (number > currentPage + 4) {
                        buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                        buttons_container.innerHTML += `<button ${number === currentPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                    }
                }
                else if (currentPage <= 4) {
                    for (let i = 1; i <= 6; i++) {
                        buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    buttons_container.innerHTML += `<button ${number === currentPage ? 'class="selected"' : ''} page="${number}"">${number}</button>`;
                }
                else if (number - currentPage <= 4) {
                    buttons_container.innerHTML += `<button ${1 === currentPage ? 'class="selected"' : ''} page="${1}"">${1}</button>`;
                    buttons_container.innerHTML += `<p class="pages_buttons-space">...</p>`;
                    for (let i = number - 5; i <= number; i++) {
                        buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                    }
                }
            }
            else {
                for (let i = 1; i <= number; i++) {
                    buttons_container.innerHTML += `<button ${i === currentPage ? 'class="selected"' : ''} page="${i}"">${i}</button>`;
                }
            }
            Array.from(buttons_container.children).forEach(button => {
                if (button.tagName === 'BUTTON')
                    button.addEventListener('click', () => updateInfo(selectedRadio, +button.getAttribute('page')));
            });
        }
    });
}
//#region Ajax requests
function getUsers() {
    fetch(`/api/v1/users/getAll?page=${usersPage}`)
        .then(response => response.json())
        .then(data => {
            data.type = "Users";
            usersCache = data;
            updatePagesButtons(data.pages);
            showData(usersCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getBotCars() {
    fetch(`/api/v1/bot/getAll?page=${botCarsPage}`)
        .then(response => response.json())
        .then(data => {
            data.type = "BotCars";
            botCarsCache = data;
            updatePagesButtons(data.pages);
            showData(botCarsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getOrders() {
    fetch(`/api/v1/orders/getAll`)
        .then(response => response.json())
        .then(data => {
            data.type = "Orders";
            ordersCache = data;
            //updatePagesButtons(data.pages);
            showData(ordersCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getWaitingCars() {
    fetch(`/Admin/GetWaitingCars?page=${waitingCarsPage}`)
        .then(response => response.json())
        .then(data => {
            waitingCarsCache = data;
            showData(waitingCarsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getBuyRequests() {
    fetch(`/Admin/GetBuyRequests?page=${buyRequestsPage}`)
        .then(response => response.json())
        .then(data => {
            buyRequestsCache = data;
            showData(buyRequestsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getBrands() {
    fetch(`/api/v1/brands/getAll`)
        .then(response => response.json())
        .then(data => {
            data.type = "Brands";
            brandsCache = data;
            showData(brandsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getExpressSaleCars() {
    fetch(`/api/v1/cars/getExpressSaleCars?page=${expressCarsPage}`)
        .then(response => response.json())
        .then(data => {
            data.type = "Express";
            expressCarsCache = data;
            showData(expressCarsCache);
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function addModel(button) {
    const input = document.getElementById('new-model-name');
    if (input != null && input.value !== '') {
        fetch(`/api/v1/brands/addModel?brand=${button.getAttribute('brand')}&model=${input.value}`, {
            method: 'POST',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true) {
                    getModelsOfMark(button.getAttribute('brand'), true);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function addBrand(button) {
    const input = document.getElementById('new-brand-name');
    if (input != null && input.value !== '') {
        fetch(`/api/v1/brands/add?brand=${input.value}`, {
            method: 'POST',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true)
                    getBrands();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function deleteBrand(button) {
    if (confirm(`Видалити марку "${button.getAttribute('brand')}"?`)) {
        fetch(`/api/v1/brands/delete?brand=${button.getAttribute('brand')}`, {
            method: 'PUT',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true)
                    getBrands();
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function deleteModel(button) {
    if (confirm(`Видалити модель "${button.getAttribute('model')}" у марки "${button.getAttribute('brand')}"?`)) {
        fetch(`/api/v1/brands/deleteModel?brand=${button.getAttribute('brand')}&model=${button.getAttribute('model')}`, {
            method: 'PUT',
        })
            .then(response => response.json())
            .then(data => {
                if (data != null && data.status == true)
                    getModelsOfMark(button.getAttribute('brand'), true);
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function editModel(button) {
    const input = document.getElementById('edit-model-name');
    if (input != null && input.value !== '') {
        let newName = input.value;
        let oldName = button.getAttribute('model');
        if (confirm(`Змінити назву моделі з "${oldName}" на "${newName}?"`)) {
            fetch(`/api/v1/brands/editModel?brand=${button.getAttribute('brand')}&newName=${newName}&oldName=${oldName}`, {
                method: 'PUT',
            })
                .then(response => response.json())
                .then(data => {
                    if (data != null && data.status == true)
                        getModelsOfMark(button.getAttribute('brand'), true);
                })
                .catch(error => console.error("An error occurred while retrieving data:", error));
        }
    }
}
function showCurrencyEditor() {
    let currency, offCurrency;
    fetch(`/api/v1/main/getCurrencyRate`)
        .then(response => response.json())
        .then(data => {
            currency = data.currencyRate;
            offCurrency = data.officialCurrencyRate;
            container.innerHTML = `<div class="currency"><p>Задати курс</p><input id="currency-inp" class="input" type="number" placeholder=${offCurrency} value=${currency}><button onclick="sendNewCurrency()">Застосувати</button></div>`;
            document.getElementById('currency-inp').addEventListener('keydown', (e) => {
                if (e.key == ',') e.preventDefault();
            });
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function sendNewCurrency() {
    fetch(`/api/v1/main/setCurrencyRate?newCurrency=${document.getElementById('currency-inp').value}`, {
        method: 'PUT'
    })
        .then(response => response.json())
        .then(data => {
            console.log(data)
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function changeModelToEditMode(button) {
    showModels(button.getAttribute('brand'));
    let modelsCont = document.getElementById('modelsCont');
    if (modelsCont == null) return;
    let brand = button.getAttribute('brand');
    let model = button.getAttribute('model');
    for (let i = 0; i < modelsCont.children.length; i++) {
        if (modelsCont.children[i].textContent.replace(/[\n\r]+|[\s]{2,}/g, '') === model) {
            modelsCont.children[i].innerHTML = `<input type="text" placeholder="Нова назва" value="${button.getAttribute('model')}" id="edit-model-name"/><div model_buttons>
                    <button onclick="editModel(this)" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-submit"><span>${svgCodes.submit}
                    </span></button><button onclick="getModelsOfMark('${brand}')" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-delete"><span>${svgCodes.cancel}
                    </span></button></div>`;
            break;
        }
    }
}
async function getModelsOfMark(brand, forced = false) {
    brand = brand.replace('_', ' ');
    if (modelsCache[brand] == null || forced) {
        fetch(`/api/v1/brands/getModels?brand=${brand}`)
            .then(response => response.json())
            .then(data => {
                data.models = data.models.filter((n) => { return n != 'Інше' });
                modelsCache[brand] = data.models;
                showModels(brand);
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else
        showModels(brand);
}
function hideModels() {
    let brandsContainer = document.getElementById('brandsCont');
    let modelsContainer = document.getElementById('modelsCont');
    modelsContainer.style.display = 'none';
    brandsContainer.style.display = 'block';
}
function showModels(brand) {
    let modelsContainer = document.getElementById('modelsCont');
    if (modelsContainer == null) return;
    modelsContainer.innerHTML = `<div class="new-model"><input type="text" placeholder="Назва моделі" id="new-model-name"/><button onclick="addModel(this)" brand="${brand}"><span>${svgCodes.add}</span>
                    </button><button onclick="hideModels()"><span>${svgCodes.cross}</span></button></div>`;
    modelsCache[brand].forEach(model => {
        modelsContainer.innerHTML += `<div class="model"><p>${model}</p><div model_buttons>
                    <button onclick="changeModelToEditMode(this)" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-edit"><span>${svgCodes.edit}
                    </span></button><button onclick="deleteModel(this)" brand="${brand.replace(' ', '_')}" model="${model.replace(' ', '_')}" class="model_buttons-delete"><span>${svgCodes.delete}
                    </span></button></div></div>`;
    });
}
//#endregion
function showNotification(message, isError = false) {
    let duration = 3000;
    activeNotifications.forEach(notification => hideNotification(notification));

    const notificationElement = document.createElement('div');
    notificationElement.classList.add('notification');
    if (isError)
        notificationElement.classList.add('notification-error');
    notificationElement.textContent = message;
    document.body.appendChild(notificationElement);
    activeNotifications.push(notificationElement);

    setTimeout(() => {
        notificationElement.style.transform = 'translateY(-125%)';
        clearTimeout(notificationTimeout);
        notificationTimeout = setTimeout(() => {
            hideNotification(notificationElement);
        }, duration);
    }, 50);

    notificationElement.addEventListener('click', () => {
        hideNotification(notificationElement);
    });
}

function hideNotification(notificationElement) {
    notificationElement.style.transform = 'translateY(10%)';
    notificationElement.style.opacity = '0.3';
    setTimeout(() => {
        notificationElement.parentNode.removeChild(notificationElement);
        const index = activeNotifications.indexOf(notificationElement);
        if (index !== -1) {
            activeNotifications.splice(index, 1);
        }
    }, 500);
}