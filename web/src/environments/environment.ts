// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  connections: {
    unsplash: {
      url: 'https://api.unsplash.com/',
      clientId: '9519e822930516201ca184e166266cc3c7afbc69ed629ad107e20f2448dcb7bc'
    },
    monatsziele: {
      url: 'http://localhost:58894/'
    }
  }
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.