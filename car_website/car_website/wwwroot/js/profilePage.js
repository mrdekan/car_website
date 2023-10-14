const svgCodes = {
    edit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_217_311)"><path d="M21 13V20C21 20.5523 20.5523 21 20 21H4C3.44771 21 3 20.5523 3 20V4C3 3.44771 3.44771 3 4 3H11" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M7 13.36V17H10.6586L21 6.65405L17.3475 3L7 13.36Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/></g><defs><clipPath id="clip0_217_311"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    submit: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_235_308)"><path d="M5 12L10 17L20 7" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_235_308"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`,
    cancel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M13.4143 12.0002L18.7072 6.70725C19.0982 6.31625 19.0982 5.68425 18.7072 5.29325C18.3162 4.90225 17.6843 4.90225 17.2933 5.29325L12.0002 10.5862L6.70725 5.29325C6.31625 4.90225 5.68425 4.90225 5.29325 5.29325C4.90225 5.68425 4.90225 6.31625 5.29325 6.70725L10.5862 12.0002L5.29325 17.2933C4.90225 17.6843 4.90225 18.3162 5.29325 18.7072C5.48825 18.9022 5.74425 19.0002 6.00025 19.0002C6.25625 19.0002 6.51225 18.9022 6.70725 18.7072L12.0002 13.4143L17.2933 18.7072C17.4883 18.9022 17.7442 19.0002 18.0002 19.0002C18.2562 19.0002 18.5122 18.9022 18.7072 18.7072C19.0982 18.3162 19.0982 17.6843 18.7072 17.2933L13.4143 12.0002Z" fill="currentColor"/></svg>`,
    driveline: `<svg version="1.0" xmlns="http://www.w3.org/2000/svg"width="24" height="24" viewBox="0 0 512.000000 512.000000"preserveAspectRatio="xMidYMid meet"><g transform="translate(0.000000,512.000000) scale(0.100000,-0.100000)" fill="currentColor" stroke="none"><path d="M950 5007 c-50 -16 -136 -103 -149 -152 -7 -26 -11 -208 -11 -531 l0 -490 28 -55 c33 -66 93 -113 162 -128 80 -17 356 -14 419 5 64 19 127 73 155 134 18 39 21 70 24 243 l4 197 143 0 142 0 6 -58 c5 -66 32 -124 74 -164 36 -35 109 -68 151 -68 18 0 40 -8 52 -20 19 -19 20 -33 20 -252 0 -268 5 -285 83 -354 50 -44 102 -64 166 -64 l41 0 0 -1180 0 -1180 -439 0 -439 0 -4 198 c-3 172 -6 203 -24 242 -28 61 -91 115 -155 134 -72 22 -364 22 -435 1 -66 -19 -121 -67 -150 -129 l-24 -51 0 -490 c0 -323 4 -504 11 -530 14 -53 99 -136 155 -153 59 -17 406 -16 457 2 57 21 100 58 132 117 29 54 29 55 33 257 l4 202 978 0 978 0 4 -202 c4 -202 4 -203 33 -257 32 -59 75 -96 132 -117 58 -20 404 -19 466 2 56 19 131 97 147 152 6 23 10 225 10 527 l0 490 -24 51 c-29 62 -84 110 -150 129 -71 21 -363 21 -435 -1 -64 -19 -127 -73 -155 -134 -18 -39 -21 -70 -24 -242 l-4 -198 -439 0 -439 0 0 1180 0 1180 43 0 c97 0 188 58 228 145 16 35 19 68 19 273 0 219 1 233 20 252 12 12 34 20 53 20 41 0 114 33 150 68 42 40 69 98 74 164 l6 58 142 0 143 0 4 -197 c3 -173 6 -204 24 -243 28 -61 91 -115 155 -134 63 -19 339 -22 419 -5 69 15 129 62 163 128 l27 55 0 490 c0 303 -4 505 -10 528 -16 55 -91 133 -147 152 -62 21 -408 22 -466 2 -57 -21 -100 -58 -132 -117 -29 -54 -29 -55 -33 -256 l-4 -203 -144 0 -144 0 0 38 c0 96 -50 182 -132 227 l-53 30 -505 0 c-502 0 -505 0 -546 -22 -87 -47 -132 -115 -141 -214 l-6 -59 -142 0 -143 0 -4 203 c-4 201 -4 202 -33 256 -32 59 -75 96 -132 117 -48 17 -411 17 -463 1z m420 -206 c6 -13 10 -180 10 -474 0 -429 -1 -455 -19 -471 -16 -14 -40 -16 -187 -14 -168 3 -168 3 -181 28 -10 19 -12 126 -11 478 3 390 5 454 18 462 8 5 92 10 187 10 160 0 173 -1 183 -19z m2750 9 c13 -8 15 -72 18 -462 1 -352 -1 -459 -11 -478 -13 -25 -13 -25 -181 -28 -147 -2 -171 0 -187 14 -18 16 -19 42 -19 471 0 294 4 461 10 474 10 18 23 19 183 19 95 0 179 -5 187 -10z m-1089 -290 c18 -10 19 -23 19 -184 0 -196 1 -192 -77 -205 -95 -15 -155 -60 -194 -146 -16 -35 -19 -69 -19 -268 0 -138 -4 -236 -10 -248 -10 -18 -23 -19 -190 -19 -167 0 -180 1 -190 19 -6 12 -10 110 -10 248 0 199 -3 233 -19 268 -39 86 -99 131 -194 146 -78 13 -77 9 -77 205 0 154 2 174 18 183 24 14 916 15 943 1z m-1669 -3257 c17 -15 18 -43 18 -470 0 -294 -4 -461 -10 -474 -10 -18 -23 -19 -183 -19 -95 0 -179 5 -187 10 -13 8 -15 72 -15 473 0 379 3 467 14 480 20 25 335 25 363 0z m2765 -13 c10 -19 12 -126 11 -478 -3 -390 -5 -454 -18 -462 -8 -5 -92 -10 -187 -10 -160 0 -173 1 -183 19 -6 13 -10 179 -10 472 0 409 2 454 17 471 15 17 32 18 187 16 l170 -3 13 -25z"/></g></svg>`,
    car: `<svg width="24" height="24" viewBox="0 0 30 30" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M24.9997 14.6876H4.9997C4.7122 14.6876 4.4497 14.5626 4.2747 14.3376C4.0997 14.1251 4.0247 13.8251 4.0872 13.5501L5.4997 6.8001C5.9622 4.6126 6.8997 2.6001 10.6122 2.6001H19.3997C23.1122 2.6001 24.0497 4.6251 24.5122 6.8001L25.9247 13.5626C25.9872 13.8376 25.9122 14.1251 25.7372 14.3501C25.5497 14.5626 25.2872 14.6876 24.9997 14.6876ZM6.1497 12.8126H23.8372L22.6622 7.1876C22.3122 5.5501 21.8997 4.4751 19.3872 4.4751H10.6122C8.0997 4.4751 7.6872 5.5501 7.3372 7.1876L6.1497 12.8126Z" fill="currentColor"/><path d="M24.9495 28.4376H22.5995C20.5745 28.4376 20.187 27.2751 19.937 26.5126L19.687 25.7626C19.362 24.8126 19.3245 24.6876 18.1995 24.6876H11.7995C10.6745 24.6876 10.5995 24.9001 10.312 25.7626L10.062 26.5126C9.7995 27.2876 9.4245 28.4376 7.3995 28.4376H5.0495C4.062 28.4376 3.112 28.0251 2.4495 27.3001C1.7995 26.5876 1.487 25.6376 1.5745 24.6876L2.2745 17.0751C2.462 15.0126 3.012 12.8126 7.0245 12.8126H22.9745C26.987 12.8126 27.5245 15.0126 27.7245 17.0751L28.4245 24.6876C28.512 25.6376 28.1995 26.5876 27.5495 27.3001C26.887 28.0251 25.937 28.4376 24.9495 28.4376ZM11.7995 22.8126H18.1995C20.4745 22.8126 21.012 23.8251 21.462 25.1501L21.7245 25.9251C21.937 26.5626 21.937 26.5751 22.612 26.5751H24.962C25.4245 26.5751 25.862 26.3876 26.1745 26.0501C26.4745 25.7251 26.612 25.3126 26.5745 24.8751L25.8745 17.2626C25.712 15.5751 25.512 14.7001 22.9995 14.7001H7.0245C4.4995 14.7001 4.2995 15.5751 4.1495 17.2626L3.4495 24.8751C3.412 25.3126 3.5495 25.7251 3.8495 26.0501C4.1495 26.3876 4.5995 26.5751 5.062 26.5751H7.412C8.087 26.5751 8.087 26.5626 8.2995 25.9376L8.5495 25.1751C8.862 24.1751 9.3245 22.8126 11.7995 22.8126Z" fill="currentColor"/><path d="M4.99951 10.9376H3.74951C3.23701 10.9376 2.81201 10.5126 2.81201 10.0001C2.81201 9.48759 3.23701 9.06259 3.74951 9.06259H4.99951C5.51201 9.06259 5.93701 9.48759 5.93701 10.0001C5.93701 10.5126 5.51201 10.9376 4.99951 10.9376Z" fill="currentColor"/><path d="M26.2495 10.9376H24.9995C24.487 10.9376 24.062 10.5126 24.062 10.0001C24.062 9.48759 24.487 9.06259 24.9995 9.06259H26.2495C26.762 9.06259 27.187 9.48759 27.187 10.0001C27.187 10.5126 26.762 10.9376 26.2495 10.9376Z" fill="currentColor"/><path d="M14.9995 7.18759C14.487 7.18759 14.062 6.76259 14.062 6.25009V3.75009C14.062 3.23759 14.487 2.81259 14.9995 2.81259C15.512 2.81259 15.937 3.23759 15.937 3.75009V6.25009C15.937 6.76259 15.512 7.18759 14.9995 7.18759Z" fill="currentColor"/><path d="M16.8745 7.18759H13.1245C12.612 7.18759 12.187 6.76259 12.187 6.25009C12.187 5.73759 12.612 5.31259 13.1245 5.31259H16.8745C17.387 5.31259 17.812 5.73759 17.812 6.25009C17.812 6.76259 17.387 7.18759 16.8745 7.18759Z" fill="currentColor"/><path d="M11.2495 19.6876H7.49951C6.98701 19.6876 6.56201 19.2626 6.56201 18.7501C6.56201 18.2376 6.98701 17.8126 7.49951 17.8126H11.2495C11.762 17.8126 12.187 18.2376 12.187 18.7501C12.187 19.2626 11.762 19.6876 11.2495 19.6876Z" fill="currentColor"/><path d="M22.4995 19.6876H18.7495C18.237 19.6876 17.812 19.2626 17.812 18.7501C17.812 18.2376 18.237 17.8126 18.7495 17.8126H22.4995C23.012 17.8126 23.437 18.2376 23.437 18.7501C23.437 19.2626 23.012 19.6876 22.4995 19.6876Z" fill="currentColor"/></svg>`,
    fuel: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M17.5 22.75H3.5C3.09 22.75 2.75 22.41 2.75 22V5.00001C2.75 2.76001 4.26 1.25001 6.5 1.25001H14.5C16.74 1.25001 18.25 2.76001 18.25 5.00001V22C18.25 22.41 17.91 22.75 17.5 22.75ZM4.25 21.25H16.75V5.00001C16.75 3.59001 15.91 2.75001 14.5 2.75001H6.5C5.09 2.75001 4.25 3.59001 4.25 5.00001V21.25Z" fill="currentColor"/><path d="M18.9999 22.75H1.99994C1.58994 22.75 1.24994 22.41 1.24994 22C1.24994 21.59 1.58994 21.25 1.99994 21.25H18.9999C19.4099 21.25 19.7499 21.59 19.7499 22C19.7499 22.41 19.4099 22.75 18.9999 22.75Z" fill="currentColor"/><path d="M12.6102 10.75H8.38023C6.75023 10.75 5.74023 9.74 5.74023 8.11V6.88C5.74023 5.25 6.75023 4.24001 8.38023 4.24001H12.6102C14.2402 4.24001 15.2502 5.25 15.2502 6.88V8.11C15.2502 9.74 14.2402 10.75 12.6102 10.75ZM8.39023 5.75C7.58023 5.75 7.25023 6.08 7.25023 6.89V8.12C7.25023 8.93 7.58023 9.25999 8.39023 9.25999H12.6202C13.4302 9.25999 13.7602 8.93 13.7602 8.12V6.89C13.7602 6.08 13.4302 5.75 12.6202 5.75H8.39023Z" fill="currentColor"/><path d="M9.49994 13.75H6.49994C6.08994 13.75 5.74994 13.41 5.74994 13C5.74994 12.59 6.08994 12.25 6.49994 12.25H9.49994C9.90994 12.25 10.2499 12.59 10.2499 13C10.2499 13.41 9.90994 13.75 9.49994 13.75Z" fill="currentColor"/><path d="M17.5 16.76C17.09 16.76 16.75 16.43 16.75 16.01C16.75 15.6 17.08 15.26 17.5 15.26L21.25 15.25V10.46L19.66 9.66998C19.29 9.47998 19.14 9.02998 19.32 8.65998C19.51 8.28998 19.96 8.13999 20.33 8.31999L22.33 9.31999C22.58 9.44999 22.74 9.70998 22.74 9.98998V15.99C22.74 16.4 22.41 16.74 21.99 16.74L17.5 16.76Z" fill="currentColor"/></svg>`,
    transmission: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M12 15.75C9.93 15.75 8.25 14.07 8.25 12C8.25 9.93 9.93 8.25 12 8.25C14.07 8.25 15.75 9.93 15.75 12C15.75 14.07 14.07 15.75 12 15.75ZM12 9.75C10.76 9.75 9.75 10.76 9.75 12C9.75 13.24 10.76 14.25 12 14.25C13.24 14.25 14.25 13.24 14.25 12C14.25 10.76 13.24 9.75 12 9.75Z" fill="currentColor"/><path d="M15.21 22.19C15 22.19 14.79 22.16 14.58 22.11C13.96 21.94 13.44 21.55 13.11 21L12.99 20.8C12.4 19.78 11.59 19.78 11 20.8L10.89 20.99C10.56 21.55 10.04 21.95 9.42 22.11C8.79 22.28 8.14 22.19 7.59 21.86L5.87 20.87C5.26 20.52 4.82 19.95 4.63 19.26C4.45 18.57 4.54 17.86 4.89 17.25C5.18 16.74 5.26 16.28 5.09 15.99C4.92 15.7 4.49 15.53 3.9 15.53C2.44 15.53 1.25 14.34 1.25 12.88V11.12C1.25 9.66 2.44 8.47 3.9 8.47C4.49 8.47 4.92 8.3 5.09 8.01C5.26 7.72 5.19 7.26 4.89 6.75C4.54 6.14 4.45 5.42 4.63 4.74C4.81 4.05 5.25 3.48 5.87 3.13L7.6 2.14C8.73 1.47 10.22 1.86 10.9 3.01L11.02 3.21C11.61 4.23 12.42 4.23 13.01 3.21L13.12 3.02C13.8 1.86 15.29 1.47 16.43 2.15L18.15 3.14C18.76 3.49 19.2 4.06 19.39 4.75C19.57 5.44 19.48 6.15 19.13 6.76C18.84 7.27 18.76 7.73 18.93 8.02C19.1 8.31 19.53 8.48 20.12 8.48C21.58 8.48 22.77 9.67 22.77 11.13V12.89C22.77 14.35 21.58 15.54 20.12 15.54C19.53 15.54 19.1 15.71 18.93 16C18.76 16.29 18.83 16.75 19.13 17.26C19.48 17.87 19.58 18.59 19.39 19.27C19.21 19.96 18.77 20.53 18.15 20.88L16.42 21.87C16.04 22.08 15.63 22.19 15.21 22.19ZM12 18.49C12.89 18.49 13.72 19.05 14.29 20.04L14.4 20.23C14.52 20.44 14.72 20.59 14.96 20.65C15.2 20.71 15.44 20.68 15.64 20.56L17.37 19.56C17.63 19.41 17.83 19.16 17.91 18.86C17.99 18.56 17.95 18.25 17.8 17.99C17.23 17.01 17.16 16 17.6 15.23C18.04 14.46 18.95 14.02 20.09 14.02C20.73 14.02 21.24 13.51 21.24 12.87V11.11C21.24 10.48 20.73 9.96 20.09 9.96C18.95 9.96 18.04 9.52 17.6 8.75C17.16 7.98 17.23 6.97 17.8 5.99C17.95 5.73 17.99 5.42 17.91 5.12C17.83 4.82 17.64 4.58 17.38 4.42L15.65 3.43C15.22 3.17 14.65 3.32 14.39 3.76L14.28 3.95C13.71 4.94 12.88 5.5 11.99 5.5C11.1 5.5 10.27 4.94 9.7 3.95L9.59 3.75C9.34 3.33 8.78 3.18 8.35 3.43L6.62 4.43C6.36 4.58 6.16 4.83 6.08 5.13C6 5.43 6.04 5.74 6.19 6C6.76 6.98 6.83 7.99 6.39 8.76C5.95 9.53 5.04 9.97 3.9 9.97C3.26 9.97 2.75 10.48 2.75 11.12V12.88C2.75 13.51 3.26 14.03 3.9 14.03C5.04 14.03 5.95 14.47 6.39 15.24C6.83 16.01 6.76 17.02 6.19 18C6.04 18.26 6 18.57 6.08 18.87C6.16 19.17 6.35 19.41 6.61 19.57L8.34 20.56C8.55 20.69 8.8 20.72 9.03 20.66C9.27 20.6 9.47 20.44 9.6 20.23L9.71 20.04C10.28 19.06 11.11 18.49 12 18.49Z" fill="currentColor"/></svg>`,
    race: `<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g clip-path="url(#clip0_14_212)"><path d="M15.1486 9.3893C15.1486 9.3893 13.534 13.9404 12.7667 14.735C11.9994 15.5296 10.7333 15.5517 9.93872 14.7844C9.14412 14.0171 9.12202 12.7509 9.88932 11.9564C10.6566 11.1618 15.1486 9.3893 15.1486 9.3893Z" stroke="currentColor" stroke-width="2" stroke-linejoin="round"/><path d="M19.4246 19.4246C21.3248 17.5245 22.5 14.8995 22.5 12C22.5 6.201 17.799 1.5 12 1.5C6.201 1.5 1.5 6.201 1.5 12C1.5 14.8995 2.67525 17.5245 4.57538 19.4246" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M12 2V4" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M19.4227 5.57105L17.8684 6.82965" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M21.2613 13.6164L19.3126 13.1665" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M2.73877 13.6164L4.68751 13.1665" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M4.57733 5.57105L6.13162 6.82965" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></g><defs><clipPath id="clip0_14_212"><rect width="24" height="24" fill="white"/></clipPath></defs></svg>`
};
const radioButtons = document.querySelectorAll('input[name="page"]');
const pageUnderline = document.getElementById('line');
const pages_buttons_containers = document.getElementsByClassName("pages_buttons");
const pages_profile = document.getElementById("pages-profile");
let cars;
let carsPage = 1;
let waitingCars;
let waitingCarsPage = 1;
let buyRequests;
let buyRequestsPage = 1;
let favCars;
let favCarsPage = 1;
let offset;
let selectedRadioButton;
let selectedRadioIndex;
function updatePagesButtons(number) {
    Array.from(pages_buttons_containers).forEach(buttons_container => {
        buttons_container.innerHTML = "";
        if (number > 1) {
            let currentPage = 1;
            if (selectedRadioButton.id == "waiting")
                currentPage = waitingCarsPage;
            else if (selectedRadioButton.id == "favorite")
                currentPage = favCarsPage;
            else if (selectedRadioButton.id == "for-sell")
                currentPage = carsPage;
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
                if (button.tagName === 'BUTTON') {
                    button.addEventListener('click', () => {
                        updateCarsList(selectedRadioButton, + button.getAttribute("page"));
                    });
                }
            });
        }
    });
}
window.addEventListener('load', function () {
    let i = 0;
    let selectedIndex;
    radioButtons.forEach(function (radio) {
        if (radio.checked) {
            selectedRadioButton = radio;
            updateCarsList(selectedRadioButton);
            selectedIndex = i;
        }
        i++;
    });
    selectedRadioIndex = selectedIndex;
    let num;
    if (window.matchMedia('(max-width: 768px)').matches)
        num = 100;
    else
        num = 66;
    pageUnderline.style.width = `${num / radioButtons.length}%`;
    offset = radioButtons.length == 4 ? 300 : 200;
    let percent = selectedIndex * (offset / (radioButtons.length - 1));
    pageUnderline.style.transform = `translateX(${percent}%)`;
});
var mediaQuery = window.matchMedia('(max-width: 768px)');
function handleMediaChange(mediaQuery) {
    let num;
    if (mediaQuery.matches)
        num = 100;
    else
        num = 66;
    pageUnderline.style.width = `${num / radioButtons.length}%`;
    offset = radioButtons.length == 4 ? 300 : 200;
    let percent = selectedRadioIndex * (offset / (radioButtons.length - 1));
    pageUnderline.style.transform = `translateX(${percent}%)`;
}
mediaQuery.addListener(handleMediaChange);
handleMediaChange(mediaQuery);
radioButtons.forEach((radio, index) => {
    radio.addEventListener('change', () => {
        offset = radioButtons.length == 4 ? 300 : 200;
        const percent = index * (offset / (radioButtons.length - 1));
        selectedRadioButton = radio;
        pageUnderline.style.transform = `translateX(${percent}%)`;
    });
});
function handleRadioChange(event) {
    updateCarsList(event.target);
}
function updateCarsList(target, page = 1) {
    if (target.id == "waiting") {
        selectedRadioIndex = 1;
        waitingCarsPage = page;
        if (waitingCars == null || waitingCars.page != waitingCarsPage)
            getWaiting();
        else {
            SetCarsFromData(waitingCars, true);
            updateLikeButtons();
            updatePagesButtons(waitingCars.pages);
        }
    }
    else if (target.id == "favorite") {
        selectedRadioIndex = 3;
        favCarsPage = page;
        if (favCars == null || favCars.page != favCarsPage)
            getFavorites();
        else {
            SetCarsFromData(favCars);
            updateLikeButtons();
            updatePagesButtons(favCars.pages);
        }
    }
    else if (target.id == "for-sell") {
        selectedRadioIndex = 0;
        carsPage = page;
        if (cars == null || cars.page != carsPage)
            getCars();
        else {
            SetCarsFromData(cars);
            updateLikeButtons();
            updatePagesButtons(cars.pages);
        }
    }
    else if (target.id == "requests") {
        selectedRadioIndex = 2;
        buyRequestsPage = page;
        if (buyRequests == null || buyRequests.page != buyRequestsPage)
            getBuyRequests();
        else {
            SetCarsFromData(buyRequests);
            updateLikeButtons();
            updatePagesButtons(buyRequests.pages);
        }
    }
}
radioButtons.forEach(function (radio) {
    radio.addEventListener('change', handleRadioChange);
});
function updateLikeButtons() {
    likeButtons = document.getElementsByClassName("car_container-right-like-cars");
    Array.from(likeButtons).forEach(like => {
        like.addEventListener('change', function (event) {
            fetch(`/car/like?carId=${event.target.getAttribute('carId')}&isLiked=${like.checked}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success == false) like.checked = !like.checked;
                })
                .catch(error => console.error("An error occurred while retrieving data:", error));
        });
    });
}
//#region Ajax requests
function getFavorites() {
    fetch(`/api/v1/users/getFavoriteCars?page=${buyRequestsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                favCars = data;
                SetCarsFromData(favCars);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getBuyRequests() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/api/v1/users/getBuyRequests?id=${userId}&page=${favCarsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                buyRequests = data;
                SetCarsFromData(buyRequests);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getWaiting() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/api/v1/users/getWaitingCars?id=${userId}&page=${waitingCarsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                waitingCars = data;
                SetCarsFromData(waitingCars, true);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
function getCars() {
    var url = new URL(window.location.href);
    var pathSegments = url.pathname.split('/');
    var userId = pathSegments[pathSegments.length - 1];
    fetch(`/api/v1/users/getSellingCars?id=${userId}&page=${carsPage}`)
        .then(response => response.json())
        .then(data => {
            if (data != null && data.status == true) {
                cars = data;
                SetCarsFromData(cars);
                updateLikeButtons();
                updatePagesButtons(data.pages);
            }
        })
        .catch(error => console.error("An error occurred while retrieving data:", error));
}
//#endregion


//#region info displaying
function SetCarsFromData(data, waitingCarsList = false) {
    const favList = document.getElementById("cars-list");
    if (data != null && data.cars.length != 0 && data.status == true) {
        favList.innerHTML = "";
        data.cars.forEach(car => {
            if (!waitingCarsList) {
                const block = formCar(car, false);
                favList.innerHTML += block;
            }
            else {
                const block = formCar(car.car, true);
                favList.innerHTML += block;
            }
        });
    }
    else if (data == null || data.status == false)
        favList.innerHTML = `<h3 class="warning-text">Щось пішло не так</h3>`;
    else
        favList.innerHTML = `<h3 class="warning-text">Тут ще нічого немає</h3>`;
}
function formCar(car, waiting) {
    return `<a class="car mainPageCar" href="/Car/Detail/${car.id}">
                                  <p class="car_name">${car.brand} ${car.model} ${car.year}</p>
                                  <div class="car_container">
                                       <div class="car_container-img"> <div class="car_container-img-landscape"><img  alt="photo" src="${car.previewURL}" /></div></div>
                                    <div class="car_container-info">
                                    <p class="car_container-info-name">${car.brand} ${car.model} ${car.year}</p>
                                            <div class="car_container-info-parameters">
                                                <div class="car_container-info-parameters-column">
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.race}</span>${car.mileage} тис. км</p>
                                                    <p class="car_container-info-parameters-column-text"><span>${svgCodes.fuel}</span>${fuelName(car.fuel)}, ${car.engineCapacity} ${car.fuel == 6 ? "кВт·год." : "л."}</p>
                                                    ${car.vin == null ? `` : `<p class="car_container-info-parameters-column-text vin"><span>${svgCodes.car}</span>${car.vin}</p>`}
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
                                            ${car.priority > 0 ? '<span class="car_container-right-top">Топ</span>' : ''}
                                  <div class="car_container-right-like">
                                 ${(waiting ? '' : `<input type="checkbox" class="car_container-right-like-cars" carId="${car.id}" carId="${car.id}" ${car.liked ? "checked" : ""}/>
                   <span class="car_container-right-heart">
                                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" clip-rule="evenodd" d="M8.272 4.95258C7.174 4.95258 6.077 5.34958 5.241 6.14458C4.441 6.90658 4 7.91758 4 8.99158C4 10.0646 4.441 11.0756 5.241 11.8376L12 18.2696L18.759 11.8376C19.559 11.0756 20 10.0646 20 8.99158C20 7.91858 19.559 6.90658 18.759 6.14458C17.088 4.55458 14.368 4.55458 12.697 6.14458L12 6.80858L11.303 6.14458C10.467 5.34958 9.37 4.95258 8.272 4.95258ZM12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>
                                  <span class="car_container-right-span"><svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M12 20.9996L3.847 13.2406C2.656 12.1076 2 10.5986 2 8.99158C2 7.38458 2.656 5.87558 3.847 4.74158C6.067 2.62858 9.552 2.43858 12 4.16858C14.448 2.43858 17.933 2.62858 20.153 4.74158C21.344 5.87558 22 7.38458 22 8.99158C22 10.5986 21.344 12.1076 20.153 13.2406L12 20.9996Z" fill="currentColor"/></svg>
                                  </span>

                   `)}
                                  </div>
                                  </div>
                                  </a>`;
}
function fuelName(id) {
    switch (id) {
        case 1:
            return "Газ";
        case 2:
            return "Газ/Бензин";
        case 3:
            return "Бензин";
        case 4:
            return "Дизель";
        case 5:
            return "Гібрид";
        case 6:
            return "Електро";
    }
}
function transmissionName(id) {
    switch (id) {
        case 1:
            return "Механічна";
        case 2:
            return "Автомат";
    }
}
function drivelineName(id) {
    switch (id) {
        case 1:
            return "Передній";
        case 2:
            return "Задній";
        case 3:
            return "Повний";
    }
}
function formatNumberWithThousandsSeparator(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
}
//#endregion
function editPhone(button) {
    let phoneCont = document.getElementById('phone_container');
    phoneCont.innerHTML = `<input type="number" placeholder="380xxxxxxxxx" value="${button.getAttribute('phone').replace('+', '')}" id="new-phone"/><button onclick="sendNewPhone(this)" id="submit-phone" phone="${button.getAttribute('phone')}"><span>
    ${svgCodes.submit}</span></button><button phone="${button.getAttribute('phone')}" onclick="cancelEditPhone(this)" id="cancel-phone"><span>${svgCodes.cancel}</span></button>`;
    let inp = document.getElementById('new-phone');
    inp.addEventListener('keydown', (event) => {
        if (event.key === "." || event.key === ",")
            event.preventDefault();
    });
    inp.addEventListener('input', () => {
        if (inp.value.length > 12)
            inp.value = inp.value.slice(0, 12);
    });
}
function editName(button) {
    let nameCont = document.getElementById('name_container');
    nameCont.innerHTML = `<input type="text" placeholder="Прізвище" value="${button.getAttribute('surname')}" id="new-surname"/><input type="text" placeholder="Ім'я" value="${button.getAttribute('name')}" id="new-name"/><button onclick="sendNewName(this)" id="submit-phone" surname="${button.getAttribute('surname')}" name="${button.getAttribute('name')}"><span>
    ${svgCodes.submit}</span></button><button surname="${button.getAttribute('surname')}" name="${button.getAttribute('name')}" onclick="cancelEditName(this)" id="cancel-phone"><span>${svgCodes.cancel}</span></button>`
}
function sendNewName(button) {
    var currentUrl = window.location.href;
    var parts = currentUrl.split("/");
    var userId = parts[parts.length - 1];
    let nameInp = document.getElementById('new-name');
    let surnameInp = document.getElementById('new-surname');
    if (nameInp != null && surnameInp != null) {
        fetch(`/api/v1/users/changeName?newName=${nameInp.value}&newSurname=${surnameInp.value}&userId=${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                clearNameError();
                let nameCont = document.getElementById('name_container');
                if (data != null && data.status == true && nameCont != null) {
                    nameCont.innerHTML = `<h2>${surnameInp.value} ${nameInp.value}</h2>
                        <button id="change-phone-number" onclick="editName(this)" surname="${surnameInp.value}" name="${nameInp.value}"><span>${svgCodes.edit}</span></button>`;
                } else if (nameCont != null) {
                    let error = document.createElement('span');
                    error.className = "text-danger";
                    error.id = 'name-error';
                    if (data != null && data.code == 400)
                        error.innerHTML = 'Некоректні дані';
                    else
                        error.innerHTML = 'Виникла помилка';
                    nameCont.insertAdjacentElement("afterend", error);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
}
function sendNewPhone(button) {
    var currentUrl = window.location.href;
    var parts = currentUrl.split("/");
    var userId = parts[parts.length - 1];
    let phoneInp = document.getElementById('new-phone');
    if (phoneInp != null && phoneInp.value != button.getAttribute('phone')) {
        fetch(`/api/v1/users/changePhone?newPhone=${phoneInp.value}&userId=${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                clearPhoneError();
                let phoneCont = document.getElementById('phone_container');
                if (data != null && data.status == true && phoneCont != null) {
                    phoneCont.innerHTML = `<h3>+${phoneInp.value}</h3>
                        <button id="change-phone-number" onclick="editPhone(this)" phone="+${phoneInp.value}"><span>${svgCodes.edit}</span></button>`;
                } else if (phoneCont != null) {
                    let error = document.createElement('span');
                    error.className = "text-danger";
                    error.id = 'phone-error';
                    if (data != null && data.code == 400)
                        error.innerHTML = 'Некоректний номер';
                    else
                        error.innerHTML = 'Виникла помилка';
                    phoneCont.insertAdjacentElement("afterend", error);
                }
            })
            .catch(error => console.error("An error occurred while retrieving data:", error));
    }
    else if (phoneInp.value == button.getAttribute('phone')) {
        let phoneCont = document.getElementById('phone_container');
        phoneCont.innerHTML = `<h3>+${phoneInp.value}</h3>
                        <button id="change-phone-number" onclick="editPhone(this)" phone="${phoneInp.value}"><span>${svgCodes.edit}</span></button>`;

    }
}
function clearPhoneError() {
    let error = document.getElementById('phone-error');
    if (error) error.remove();
}
function clearNameError() {
    let error = document.getElementById('name-error');
    if (error) error.remove();
}
function cancelEditName(button) {
    clearNameError();
    let nameCont = document.getElementById('name_container');
    nameCont.innerHTML = `<h2>${button.getAttribute('surname')} ${button.getAttribute('name')}</h2>
        <button id="change-phone-number" onclick="editName(this)" surname="${button.getAttribute('surname')}" name="${button.getAttribute('name')}"><span>
            ${svgCodes.edit}
        </span></button>`;
}
function cancelEditPhone(button) {
    clearPhoneError();
    let phoneCont = document.getElementById('phone_container');
    phoneCont.innerHTML = `<h3>${button.getAttribute('phone')}</h3>
        <button id="change-phone-number" onclick="editPhone(this)" phone=${button.getAttribute('phone')}><span>
            ${svgCodes.edit}
        </span></button>`;
}
function copyPhone(button) {
    var textToCopy = button.getAttribute('phone');
    var tempTextArea = document.createElement("textarea");
    tempTextArea.value = textToCopy;
    document.body.appendChild(tempTextArea);
    tempTextArea.select();
    tempTextArea.setSelectionRange(0, 99999);
    document.execCommand("copy");
    document.body.removeChild(tempTextArea);
}