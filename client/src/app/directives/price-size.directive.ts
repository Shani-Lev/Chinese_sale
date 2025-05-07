import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appPriceSize]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: PriceSizeDirective,
      multi: true,
    },],
  standalone: true,
})
export class PriceSizeDirective {

  constructor() { }
  minSize: number = 1;

  validate(control: AbstractControl): ValidationErrors | null {
    const price = control.get('price')?.value;
    const size = control.get('size')?.value || control.get('size')?.value;
    console.log(price, size);
    
    if (price > 20 && size === this.minSize) {
      console.log("a");
      return { sizeInvalid: true };  
    }
    // else return { sizeInvalid: true }; 
    console.log("b");
    
    return null;
  }
}
