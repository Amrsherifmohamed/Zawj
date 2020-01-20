/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MassegasComponent } from './massegas.component';

describe('MassegasComponent', () => {
  let component: MassegasComponent;
  let fixture: ComponentFixture<MassegasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MassegasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MassegasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
