# Tiles

[Docs](https://docs.google.com/document/d/19SKNV5MEHqLctydN1dpG4akLNLmEb8D7xi8h2gvxH0E/edit#)

## Development 
### Dependencies

- [Git](https://git-scm.com/downloads)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Unity 2022.2.18f1](https://unity.com/download)

### Setup

1. Start by installing unity hub and git. In unity hub install the 2022.2.141f version of unity.
2. With git installed you connect your github account with your personal computer through SSH. Do so by following this link: https://docs.github.com/en/authentication/connecting-to-github-with-ssh
3. Now you can clone the project by running this command in your termina: `git@github.com:Jaspero/tiles-game`.
4. This will create a `tiles-game` folder in your working directory. You should import it in to unity by using the "Add project from disk option"
5. It's recommended to use Visual Studio Code for writting C# in unity. When that's installed you can follow this guide to connect it with unity: https://code.visualstudio.com/docs/other/unity

### Setting Up Firebase

For firebase to work you'll need to add a keystore file to your unity project. 

1. Open the project settings in unity then click on "Keystore Manager" in the "Player" segment.
2. Create a new keystore `user.keystore` in the root of the repository.
3. Run `keytool -list -keystore user.keystore` in root to generate a fingerprint. Add the fingerprint in firebase [here](https://console.firebase.google.com/u/1/project/jaspero-tiles/settings/general/android:com.jaspero.tiles).

**Note:** Firebase has already been added to the project and you shouldn't need to add it again adding this just in case.

1. Download the [Unity SDK](https://firebase.google.com/download/unity?hl=en&authuser=1&_gl=1*g20k4x*_ga*MzE5MzA1Mzg1LjE2NzgwMTQ5MTE.*_ga_CW55HF8NVT*MTY4NzAzMjg3MS4xNDMuMS4xNjg3MDM0NzY1LjAuMC4w).
2. Unzip it and in unity do the following "Assets" > "Import Packages" > "Custom Package"
3. Now import the "FirebaseFirestore.unitypackage"

## Useful Links

- [Firebase Project](https://console.firebase.google.com/u/1/project/jaspero-tiles)
- [Firebase Unity SDK](https://firebase.google.com/download/unity?hl=en&authuser=1&_gl=1*g20k4x*_ga*MzE5MzA1Mzg1LjE2NzgwMTQ5MTE.*_ga_CW55HF8NVT*MTY4NzAzMjg3MS4xNDMuMS4xNjg3MDM0NzY1LjAuMC4w)